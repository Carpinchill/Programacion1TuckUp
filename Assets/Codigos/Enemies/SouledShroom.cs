using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouledShroom : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float currentHealth;          //salud del enemigo
    public float maxHealth = 33f;

    public Movement player;             //ref al player (posición para takedamage
    public Attack playerAttack;         //ref al script del ataque del jugador
    public float playerNear = 10f;      //cuán cerca tiene que estar el jugador

    public SS_Bullet EYBullet;          //ref al prefab de la bala
    public Transform EYBulletSpawn;     //ref al transform del bullet spawn point
    public GameObject shardPrefab;      //ref al prefab de los shards    

    public float bulletFrequency = 3f;  //cada cuánto se dispara cada bala
    private float lastShot = 0f;        //tiempo desde el último disparo

    private AudioSource audioSource;
    public AudioClip enemyHurtSound, enemyDeathSound, shootSound;
    public float minPitch = 0.80f;
    public float maxPitch = 1.25f;

    //--------------------------------- V. A W A K E ---------------------------------//

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    //--------------------------------- V. S T A R T ---------------------------------//

    private void Start()
    {
        currentHealth = maxHealth;
    }

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
            SS_Bullet bulletNew = Instantiate(EYBullet, EYBulletSpawn.position, EYBulletSpawn.rotation);
            bulletNew.Shooting(bulletToPlayer);

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(shootSound);
        }                                    
    }

    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;           //restamos el daño a la salud del enemigo

        audioSource.PlayOneShot(enemyHurtSound);

        if (currentHealth <= 0)            //si la salud es menos que 0
        {
            if (Random.value <= 0.5f)
            {
                Instantiate(shardPrefab, transform.position, Quaternion.identity);
            }

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(enemyDeathSound);


            Destroy(gameObject);    //muere
        }
    }
}
