using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ : MonoBehaviour
{
    public float speed = 5f;         //velocidad del enemigoZ

    public EZ_bullet ZBullet;        //prefab bullet
    public Transform EZ_spawnPointL; //spawn izq
    public Transform EZ_spawnPointR; //spawn dcha

    public Transform[] waypoints;   //array para almacenar waypoints
    int currentWaypoint = 0;        //para ubicar en qué waypoint estamos

    // Start is called before the first frame update
    void Start()
    {
        /*INSTANCIAR BALA
        if(jugador está cerca)
        {
            EZ_bullet newBullet = Instantiate(EZ_bullet, EZ_spawnPointL.position, transform.rotation);
            newBullet.EnemyZ = this;
        }*/
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
