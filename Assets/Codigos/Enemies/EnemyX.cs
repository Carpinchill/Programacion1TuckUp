using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public Movement player;         //ref al player para obtener sus datos
    private Attack playerAttack;

    public float health = 50f;      //salud del enemigo
    public float damage = 10f;      //daño que hace el enemigo
    public float speed = 5f;        //velocidad del enemigo
    public float knockBackforce = 0f;

    public Transform[] waypoints;   //array para almacenar waypoints
    int currentWaypoint = 0;        //para ubicar en qué waypoint estamos
    public float playerNear = 5f;   //cuán cerca tiene que estar el jugador para que lo detecte


    private void Start()
    {
        playerAttack = player.GetComponent<Attack>();
    }

    //---------------------------------- V. U P D A T E ----------------------------------//


    void Update()
    {
        //si está cerca del jugador, que se mueva hacia él
        if(Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
        {
            //dirección del movimiento (hacia el jugador)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, speed * Time.deltaTime);

            //cambio de posición
            transform.position = newPosition;
        }
        //si no, que patrulle
        else
        {
            Patrol();
        }
    }

    //---------------------------------- M E T H O D S ----------------------------------// 

    //--- PATRULLAR ---//
    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)   //si el enemigo está muy cerca de un waypoint
        {
            currentWaypoint++;                                                                  //el waypoint actual pasa a ser el siguiente
            if (currentWaypoint >= waypoints.Length)                                            //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0;                                                            //vuelve al primero
            }
        }

        //dirección del movimiento (hacia el siguiente waypoint)
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        //cambio de posición
        transform.position = newPosition;
    }

    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        health -= damage;
        Debug.Log("Enemy received " + damage + " damage.");
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        if (health <= 0)
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            if (playerAttack != null)
            {
                float damage = playerAttack.damage;
                float knockbackForce = playerAttack.knockbackForce;
                ReceiveDamage(damage, knockbackDirection, knockbackForce);
            }
        }
    }
    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy died.");
    }

    //--- HACER DAÑO AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 impactSource = transform.position;
            float knockbackForce = 1000f;
            if (collision.gameObject.TryGetComponent<Movement>(out var playerMovement))
            {
                playerMovement.TakeDamage(damage, impactSource, knockbackForce);
            }
        }
    }
}
