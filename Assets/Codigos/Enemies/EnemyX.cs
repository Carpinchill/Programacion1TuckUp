using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemyX : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public Player player;                       //ref al script del jugador
    public P_Attack playerAttack;               //ref al script del ataque del jugador

    public float health = 43f;                  //salud del enemigo
    public float damage = 10f;                  //daño que hace el enemigo
    public float speed = 5f;                    //velocidad del enemigo
        
    public Transform[] waypoints;               //array para almacenar waypoints
    int currentWaypoint = 0;                    //waypoint actual
    public float playerNear = 5f;               //cuán cerca tiene que estar el jugador para ser detectado
        
    public float attackCooldown = 2f;           //tiempo hasta volver a atacar
    public float knockbackForceEnemy = 50f;     //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    //private bool hasCollided = false;           //pregunta si colisionó con el jugador
    private Vector3 collisionDirection;         //dirección en la que se aleja luego de colisionar


    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<P_Attack>();           //obtenemos el ataque del script del jugador                
    }

    //--------------------------------- V. U P D A T E ---------------------------------//

    //--- RUTINA DEL ENEMIGO X ---//
    void Update()
    {
        //si el jugador está cerca
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
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
        //dirección del mov (hacia el jugador)
        Vector3 directionToPlayer = (player.gameObject.transform.position - transform.position).normalized;

        //velocidad del mov
        transform.position += speed * Time.deltaTime * directionToPlayer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement player = collision.gameObject.GetComponentInParent<Movement>();
        if (player != null)
        {
            Debug.Log("The Blue Enemy has collided with player");
            Vector2 impactSource = transform.position;                  //guardamos la posición el enemigo en el momento del impacto           
            player.TakeDamage(damage, impactSource, knockbackForceEnemy);    //llamamos el método TakeDamage DEL JUGADOR                       
            transform.position -= speed * Time.deltaTime * collisionDirection;  //alejarse después de colisionar
        }
    }

    //--- RECIBIR ATAQUE DEL JUGADOR ---//   
    public void TakeDamage(float damage, float knockbackForce, Vector3 knockbackDirection)
    {
        health -= damage;                                       //restamos el damage a la salud del enemigo
        Debug.Log("Enemy received " + damage + " damage.");     //print del damage

        if (TryGetComponent<Rigidbody2D>(out var rb))           //obtenemos el RigidBody del enemigo
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);    //le aplicamos el knockback
        }
        if (health <= 0)        //comprobamos vida, si se acabó
        {
            Die();              //muereee :(
        }
    }

    //--- MUEREE!!! ---//
    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("This enemy's gone to heaven.");
    }
}
