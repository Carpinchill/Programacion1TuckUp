using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyY : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 33f;          //salud del enemigo

    public Movement player;             //ref al player (posición para takedamage
    public Attack playerAttack;         //ref al script del ataque del jugador
    public float playerNear = 10f;      //cuán cerca tiene que estar el jugador

    public EY_Bullet EYBullet;          //ref al prefab de la bala
    public Transform EYBulletSpawn;     //ref al transform del bullet spawn point

    public float bulletFrequency = 3f;  //cada cuánto se dispara cada bala
    private float lastShot = 0f;        //tiempo desde el último disparo
    

    //--------------------------------- V. U P D A T E ---------------------------------//
    void Update()
    {
        //si el jugador está cerca, dispara
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
        {
            if (Time.time >= lastShot + bulletFrequency)
            {
                Shoot();      //llamamos a la función Shoot
                lastShot = Time.time;   //actualiza el tiempo del último disparo
            }
        }        
    }


    //--------------------------------- M E T H O D S ---------------------------------//

    //--- DISPARAR ---//
    void Shoot()
    {
        if (EYBulletSpawn != null)
        {
            Vector3 bulletToPlayer = (player.transform.position - EYBulletSpawn.position).normalized;
            EY_Bullet bulletNew = Instantiate(EYBullet, EYBulletSpawn.position, EYBulletSpawn.rotation);
            bulletNew.Shooting(bulletToPlayer);
        }                                    
    }

    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage)
    {
        health -= damage;           //restamos el daño a la salud del jugador

        if (health <= 0)            //si la salud es menos que 0
        {
            Destroy(gameObject);    //muere
        }
    }
}
