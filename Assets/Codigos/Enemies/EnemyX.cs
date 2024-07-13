using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemyX : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 43f;                  //salud del enemigo
    public float damage = 6f;                  //daño que hace el enemigo
    public float speed = 9f;                    //velocidad del enemigo

    public Movement player;                     //ref al script del jugador
    public Attack playerAttack;                 //ref al script del ataque del jugador   

    public Transform[] waypoints;               //array para almacenar waypoints
    int currentWaypoint = 0;                    //waypoint actual
    public float playerNear = 5f;               //cuán cerca tiene que estar el jugador para ser detectado
        
    public float attackCooldown = 2f;           //tiempo hasta volver a atacar
    public float knockbackForceEnemy = 2000f;     //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    private bool isKnockback = false;           //pregunta si si está retrocediendo

    public float lastH, lastV;
    private Animator animatorEX;
    private Vector3 lastPosition;

    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>();           //obtenemos el ataque del jugador
        animatorEX = GetComponent<Animator>();
        lastPosition = transform.position;
    }

<<<<<<< Updated upstream

=======
    //--------------------------------- L. U P D A T E ---------------------------------//

    private void LateUpdate()
    {
        animatorEX.SetFloat("LastH", lastH);
        animatorEX.SetFloat("LastV", lastV);
    }
>>>>>>> Stashed changes
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
        UpdateAnimatorParameters();
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
        //pasamos los parametros al animator
        UpdateAnimatorParameters();

        //guardamos la posición del jugador
        Vector3 directionToPlayer = (player.gameObject.transform.position - transform.position).normalized;

        //mov HACIA el jugador
        transform.position += speed * Time.deltaTime * directionToPlayer;
    }
    void UpdateAnimatorParameters()
    {
        Vector3 movement = transform.position - lastPosition;

        if (movement != Vector3.zero)
        {
            //normalizamos el movimiento para obtener solo la dirección
            Vector3 normalizedMovement = movement.normalized;
            animatorEX.SetFloat("LastH", normalizedMovement.x);
            animatorEX.SetFloat("LastV", normalizedMovement.y);
        }

        //actualiza la última posición
        lastPosition = transform.position;
    }

    //--- COLISIONAMOS CON JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement player = collision.gameObject.GetComponentInParent<Movement>();
        if (player != null)
        {
            Vector3 collisionDirection = (transform.position - collision.transform.position).normalized; //guardamos la dirección (opuesta al punto de colisión)
            StartCoroutine(Knockback(collisionDirection)); //retrocedemos con la dirección que guardamos recién
        }
    }


    //--- RETROCESO ---//
    IEnumerator Knockback(Vector3 direction)
    {
        isKnockback = true;
        float knockbackTime = 0.7f; // duración del retroceso
        float timer = 0f;


        player.TakeDamage(damage, direction, knockbackForceEnemy);

        while (timer < knockbackTime)
        {
            transform.position += speed * Time.deltaTime * direction;
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


    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage)
    {
        health -= damage;           //restamos el daño a la salud del jugador

        if (health <= 0)            //si la salud es menos que 0
        {
            Destroy(gameObject);    //muere
            Debug.Log("This enemy's gone to hell");
        }
    }
}
