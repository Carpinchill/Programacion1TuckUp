using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OA_Bullet : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float lifetime = 9f;                   //vida de la bala
    public float bulletSpeed = 6f;               //velocidad de la bala
    private Vector3 bulletDirection;              //direcci�n de la bala ??

    public float damage = 10f;                    //da�o que hace la bala
    public float knockbackForceBullet = 20f;      //retroceso aplicado POR LA BALA -> AL JUGADOR


    //------------------------------------ V. S T A R T ---------------------------------//
    void Start()
    {
        Destroy(gameObject, lifetime);      //la bala se autodestruye despu�s de un tiempo determinado
    }

    //----------------------------------- V. U P D A T E ---------------------------------//
    void Update()
    {
        transform.position += bulletSpeed * Time.deltaTime * bulletDirection;    //movimiento de la bala
    }


    //--------------------------------- M E T H O D S ---------------------------------//

    //--- SETEAMOS DIRECCI�N DE LA BALA SEG�N EL VECTOR 3 QUE OBTENEMOS DE OA ---//
    public void Shooting(Vector3 dir)
    {
        bulletDirection = dir;
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
