using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 100f;         //salud del enemigo

    public float speed = 15f;           //velocidad del enemigo
    
    public Transform[] waypoints;       //array para almacenar waypoints
    int currentWaypoint = 0;            //para ubicar en qué waypoint estamos

    public Movement player;             //ref al player para obtener sus datos
    public float playerNear = 1f;       //cuán cerca tiene que estar el jugador para que lo detecte

    private EZ_Shooter[] EZShooter;   //array de bullet spawns


    //--------------------------------- A W A K E ---------------------------------//
    private void Awake()
    {
        EZShooter = GetComponentsInChildren<EZ_Shooter>();
    }
    
    
    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        
    }

    
    //called ONCE PER FRAME
    void Update()
    {
        //si está cerca del jugador, que se mueva hacia él
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
        {
            //dirección del movimiento (hacia el jugador)
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, speed * Time.deltaTime);

            //cambio de posición
            transform.position = newPosition;

            //dispara
            for (int i = 0; i < EZShooter.Length; i++)
            {
                EZShooter[i].Shoot();
            }
        }
        //si no, que patrulle
        else
        {
            Patrol();
        }        
    }

    //--------------------------------- M E T H O D S ---------------------------------//

    //--- RECIBIR DAÑO ---//
    public void GetDamage(float amount)
    {
        health -= amount;
        print("El Troll recibió " + amount + " de daño y su vida actual es de " + health);

        if (health <= 0)
        {
            print("El Troll se murió");
            Destroy(gameObject);
        }
    }


    //--- PATRULLAR ---//
    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)   //si el enemigo está muy cerca de un waypoint
        {
            currentWaypoint++;                                                                  //pasa al siguiente
            if (currentWaypoint >= waypoints.Length)                                            //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0;                                                            //vuelve al primero
            }
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        transform.position = newPosition;
    }
}
