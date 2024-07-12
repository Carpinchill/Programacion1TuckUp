using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EY_Bullet : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float lifetime = 10f;                  //vida de la bala
    public float bulletSpeed;                     //velocidad de la bala
    private Vector3 bulletDirection;              //direcci�n de la bala

    public float damage = 5f;                     //da�o que hace la bala
    public float knockbackForceBullet = 10f;      //retroceso aplicado POR LA BALA -> AL JUGADOR    


    //------------------------------------ V. S T A R T ---------------------------------//
    void Start()
    {
        Destroy(gameObject, lifetime);      //la bala se autodestruye despu�s de un tiempo determinado
    }

    //----------------------------------- V. U P D A T E ---------------------------------//
    void Update()
    {
        transform.position += (Vector3)(bulletDirection * bulletSpeed * Time.deltaTime);
    }


    //--------------------------------- M E T H O D S ---------------------------------//

    //--- SETEAMOS DIRECCI�N Y VELOCIDAD ---//
    public void Initialize(Vector3 dir, float speed)
    {
        bulletDirection = dir;
        bulletSpeed = speed;
    }


    //--- HACER DA�O AL TOCAR AL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)                      //detecta colisi�n
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))      //si colisiona con la capa del player
        {
            Vector2 impactSource = transform.position;                          //obtenemos la posici�n del impacto

            //llamamos al m�todo TakeDamage DEL JUGADOR para pasarle el da�o y el lugar de impacto
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage, impactSource, knockbackForceBullet);
        }

        Destroy(gameObject);                                                    //por haber colisionado, se autodestruye
    }
}
