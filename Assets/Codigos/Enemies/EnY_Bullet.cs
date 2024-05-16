using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnY_Bullet : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public float speed = 5f;        //velocidad de la bala

    public float lifetime = 2f;     //vida de la bala

    public float damage = 5f;      //daño que hace la bala

    //public EnemyY enemyY;


    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        Destroy(gameObject, lifetime);      //la bala se autodestruye después de un tiempo determinado
    }

    //called ONCE PER FRAME
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;    //movimiento de la bala
    }


    //--- HACER DAÑO AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)                      //detecta colisión
    {
        if (collision.gameObject.layer == 6)                                    //si colisiona con la capa del player
        {
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage);   //le hace daño al jugador
            print("El enemigo azul le hizo " + damage + " daño al jugador");
        }
        Destroy(gameObject);                                                    //por haber detectado colisión, se autodestruye
    }
}
