using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyY : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 33f;          //salud del enemigo

    public Movement player;             //ref al player (posici�n para takedamage
    public Attack playerAttack;         //ref al script del ataque del jugador
    public float playerNear = 10f;      //cu�n cerca tiene que estar el jugador

    public EY_Bullet EYBullet;          //ref al prefab de la bala
    public Transform EYBulletSpawn;     //ref al transform del bullet spawn point

    public float bulletFrequency = 3f;  //cada cu�nto se dispara cada bala
    private float lastShot = 0f;        //tiempo desde el �ltimo disparo
    

    //--------------------------------- V. U P D A T E ---------------------------------//
    void Update()
    {
        //si el jugador est� cerca, dispara
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
        {
            if (Time.time >= lastShot + bulletFrequency)
            {
                Shoot();      //llamamos a la funci�n Shoot del Shooter
                lastShot = Time.time;   //actualiza el tiempo del �ltimo disparo
            }
        }        
    }


    //--------------------------------- M E T H O D S ---------------------------------//

    //--- DISPARAR ---//
    void Shoot()
    {
        Instantiate(EYBullet, EYBulletSpawn.position, transform.rotation);
    }
    
    //--- RECIBIR ATAQUE DEL JUGADOR ---//
    private void OnTriggerEnter2D(Collider2D other)
    {
        //si se colision� con la capa PlayerHitbox Y el ataque NO es nulo
        if ((other.gameObject.layer == LayerMask.NameToLayer("HitboxPlayer")) && (playerAttack != null))
        {
            float damage = playerAttack.damage;                     //obtenemos el damage del script del jugador
            
            health -= damage;                                       //restamos el damage a la salud del enemigo
            Debug.Log("SeedSpitter received " + damage + " damage.");     //print del damage
                        
            if (health <= 0)        //comprobamos vida, si se acab�
            {
                Die();              //muereee :(
            }
        }
    }


    //--- MUEREE!!! ---//
    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("This enemy's gone to hell."); //pal infierno causita
    }
}
