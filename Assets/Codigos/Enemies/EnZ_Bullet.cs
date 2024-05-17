using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnZ_Bullet : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public float speed = 5f;        //velocidad de la bala
    public float lifetime = 5f;     //vida de la bala
    public float damage = 15f;      //da�o que hace la bala
    public float knockBackforce = 0f;

    //public EnemyZ enemyZ;

    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        Destroy(gameObject, lifetime);      //la bala se autodestruye despu�s de un tiempo determinado
    }

    //called ONCE PER FRAME
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;    //movimiento de la bala
    }


    //--- HACER DA�O AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)                      //detecta colisi�n
    {
        if (collision.gameObject.layer == 6)                                    //si colisiona con la capa del player
        {
            Vector2 impactSource = transform.position;
            float knockbackForce = 10f;
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage, impactSource, knockbackForce);   //le hace da�o al jugador
        }
        Destroy(gameObject);                                                    //por haber detectado colisi�n, se autodestruye
    }
}
