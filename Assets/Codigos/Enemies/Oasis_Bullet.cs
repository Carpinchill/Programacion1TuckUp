using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oasis_Bullet : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float lifetime = 10f;                  //vida de la bala
    public float bulletSpeed;                     //velocidad de la bala
    private Vector2 direction;                    //dirección de la bala

    public float damage = 5f;                     //daño que hace la bala
    public float knockbackForceBullet = 10f;      //retroceso aplicado POR LA BALA -> AL JUGADOR


    //------------------------------------ V. S T A R T ---------------------------------//
    void Start()
    {
        Destroy(gameObject, lifetime);      //la bala se autodestruye después de un tiempo determinado
    }

    //----------------------------------- V. U P D A T E ---------------------------------//
    void Update()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;    //movimiento de la bala
    }


    //--------------------------------- M E T H O D S ---------------------------------//

    //--- SETEAMOS DIRECCIÓN Y VELOCIDAD ---//
    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir;
        bulletSpeed = speed;
    }


    //--- HACER DAÑO AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)                      //detecta colisión
    {
        if (collision.gameObject.layer == 6)                                    //si colisiona con la capa del player
        {
            Vector2 impactSource = transform.position;
            float knockbackForce = 10f;
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage, impactSource, knockbackForce);   //le hace daño al jugador
        }
        Destroy(gameObject);                                                    //por haber detectado colisión, se autodestruye
    }
}
