using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemyX : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 43f;                  //salud del enemigo
    public float damage = 10f;                  //daño que hace el enemigo
    public float speed = 5f;                    //velocidad del enemigo

    public Movement player;                     //ref al script del jugador
    public Attack playerAttack;                 //ref al script del ataque del jugador
       
    public Transform[] waypoints;               //array para almacenar waypoints
    int currentWaypoint = 0;                    //waypoint actual
    public float playerNear = 5f;               //cuán cerca tiene que estar el jugador para ser detectado
        
    public float attackCooldown = 2f;           //tiempo hasta volver a atacar
    public float knockbackForceEnemy = 50f;     //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    private bool isKnockback = false;           //pregunta si si está retrocediendo


    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>();           //obtenemos el ataque del jugador                
    }

    //--------------------------------- V. U P D A T E ---------------------------------//

    //--- RUTINA DEL ENEMIGO X ---//
    void Update()
    {        
        if (isKnockback) return;    //si está en retroceso, que no haga nada más
        else if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)    //si el jugador está cerca
        {
            Attack();       //que ataque
        }
        else                //si no, que patrulle
        {
            Patrol();
        }
    }

    //--------------------------------- M E T H O D S ---------------------------------//

    //--- PATRULLAR ---//
    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)   //si el enemigo está muy cerca de un waypoint
        {
            currentWaypoint++;                               //el waypoint actual pasa a ser el siguiente
            if (currentWaypoint >= waypoints.Length)         //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0;                         //vuelve al primero
            }
        }

        //dirección del mov (hacia siguiente waypoint)
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        //cambio de posición
        transform.position = newPosition;
    }


    //--- ATACAR ---//
    void Attack()
    {
        //guardamos la posición del jugador
        Vector3 directionToPlayer = (player.gameObject.transform.position - transform.position).normalized;

        //mov HACIA el jugador
        transform.position += speed * Time.deltaTime * directionToPlayer;
    }


    //--- COLISIONAMOS CON JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement player = collision.gameObject.GetComponentInParent<Movement>();
        if (player != null)
        {
            Debug.Log("The Blue Enemy has collided with player");
            Vector3 collisionDirection = (transform.position - collision.transform.position).normalized; //guardamos la dirección (opuesta al punto de colisión)
            StartCoroutine(Knockback(collisionDirection)); //retrocedemos con la dirección que guardamos recién
        }
    }


    //--- RETROCESO ---//
    IEnumerator Knockback(Vector3 direction)
    {
        isKnockback = true;
        float knockbackTime = 1f; // duración del retroceso
        float timer = 0f;

        while (timer < knockbackTime)
        {
            transform.position += direction * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;

        //decidimos qué hacer después del retroceso
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)    //si el jugador está cerca
        {
            Attack(); //que ataque
        }
        else      //si NO está cerca
        {
            currentWaypoint = 0;    //el waypoint actual vuelve a ser el primero
            Patrol();               //que patrulle
        }
    }    
}
