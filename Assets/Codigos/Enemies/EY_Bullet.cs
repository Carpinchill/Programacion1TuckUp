using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EY_Bullet : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float lifetime = 10f;                  //vida de la bala
    public float bulletSpeed;                     //velocidad de la bala
    private Vector3 bulletDirection;              //dirección de la bala

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
        transform.position += (Vector3)(bulletDirection * bulletSpeed * Time.deltaTime);
    }


    //--------------------------------- M E T H O D S ---------------------------------//

    //--- SETEAMOS DIRECCIÓN Y VELOCIDAD ---//
    public void Initialize(Vector3 dir, float speed)
    {
        bulletDirection = dir;
        bulletSpeed = speed;
    }


    //--- HACER DAÑO AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)                      //detecta colisión
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))      //si colisiona con la capa del player
        {
            Vector2 impactSource = transform.position;                          //obtenemos la posición del impacto

            //llamamos al método TakeDamage DEL JUGADOR para pasarle el daño y el lugar de impacto
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage, impactSource, knockbackForceBullet);
        }

        Destroy(gameObject);                                                    //por haber colisionado, se autodestruye
    }
}
