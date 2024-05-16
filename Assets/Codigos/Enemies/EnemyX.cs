using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public float health = 50f;      //salud del enemigo

    public float damage = 10f;      //da�o que hace el enemigo

    public float speed = 5f;        //velocidad del enemigo
    public Transform[] waypoints;   //array para almacenar waypoints
    int currentWaypoint = 0;        //para ubicar en qu� waypoint estamos

    public Movement player;         //ref al player para obtener sus datos
    public float playerNear = 1f;   //cu�n cerca tiene que estar el jugador para que lo detecte
        

    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        
    }

    //called ONCE PER FRAME
    void Update()
    {
        //si est� cerca del jugador, que se mueva hacia �l
        if(Vector3.Distance(transform.position, player.gameObject.transform.position) < 1f)
        {
            //direcci�n del movimiento (hacia el jugador)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, speed * Time.deltaTime);

            //cambio de posici�n
            transform.position = newPosition;
        }
        //si no, que patrulle
        else
        {
            Patrol();
        }
    }

    //---------------------------------- M E T H O D S ----------------------------------//

    //--- RECIBIR DA�O ---//
    public void GetDamage(float amount)
    {
        health -= amount;
        print("El enemigo azul recibi� " + amount + " de da�o y su vida actual es de " + health);

        if (health <= 0)
        {
            print("El enemigo azul se muri�");
            Destroy(gameObject);
        }
    }
    
    
    //--- PATRULLAR ---//
    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)   //si el enemigo est� muy cerca de un waypoint
        {
            currentWaypoint++;                                                                  //el waypoint actual pasa a ser el siguiente
            if (currentWaypoint >= waypoints.Length)                                            //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0;                                                            //vuelve al primero
            }
        }

        //direcci�n del movimiento (hacia el siguiente waypoint)
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        //cambio de posici�n
        transform.position = newPosition;
    }


    //--- HACER DA�O AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage);
            print("El enemigo azul le hizo " + damage + " da�o al jugador");
        }
    }
}
