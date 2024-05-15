using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed = 5f;        //velocidad del enemigoX

    public Transform[] waypoints;   //array para almacenar waypoints
    int currentWaypoint = 0;        //para ubicar en qué waypoint estamos

    /*public PlayerRigidbodyMovement player;*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

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
