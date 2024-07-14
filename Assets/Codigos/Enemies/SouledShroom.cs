using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouledShroom : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 33f;          //salud del enemigo

    public Movement player;             //ref al player (posici�n para takedamage
    public Attack playerAttack;         //ref al script del ataque del jugador
    public float playerNear = 10f;      //cu�n cerca tiene que estar el jugador

    public SS_Bullet EYBullet;          //ref al prefab de la bala
    public Transform EYBulletSpawn;     //ref al transform del bullet spawn point
    public GameObject shardPrefab;      //ref al prefab de los shards    

    public float bulletFrequency = 3f;  //cada cu�nto se dispara cada bala
    private float lastShot = 0f;        //tiempo desde el �ltimo disparo

    private AudioSource audioSource;
    public AudioClip enemyHurtSound, enemyDeathSound, shootSound;

    //--------------------------------- V. U P D A T E ---------------------------------//
    void Update()
    {
        //si el jugador est� cerca, dispara
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
        {
            if (Time.time >= lastShot + bulletFrequency)
            {
                Shoot();      //llamamos a la funci�n Shoot
                lastShot = Time.time;   //actualiza el tiempo del �ltimo disparo
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
            SS_Bullet bulletNew = Instantiate(EYBullet, EYBulletSpawn.position, EYBulletSpawn.rotation);
            bulletNew.Shooting(bulletToPlayer);

            audioSource.PlayOneShot(shootSound);
        }                                    
    }

    //--- RECIBIR DA�O ---//
    public void ReceiveDamage(float damage)
    {
        health -= damage;           //restamos el da�o a la salud del enemigo

        audioSource.PlayOneShot(enemyHurtSound);

        if (health <= 0)            //si la salud es menos que 0
        {
            if (Random.value <= 0.5f)
            {
                Instantiate(shardPrefab, transform.position, Quaternion.identity);
            }

            audioSource.PlayOneShot(enemyDeathSound);


            Destroy(gameObject);    //muere
        }
    }
}
