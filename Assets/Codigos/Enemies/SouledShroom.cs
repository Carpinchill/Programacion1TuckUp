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
    public float playerNear = 7f;       //cuán cerca tiene que estar el jugador

    public SS_Bullet SSBullet;          //ref al prefab de la bala
    public Transform SSBulletSpawn;     //ref al transform del bullet spawn point
    public GameObject shardPrefab;      //ref al prefab de los shards    
    public Transform healthBar;                 //ref a la barra de vida
    private Vector3 healthBarOriginalScale;     //ref a la escala original de la barra de vida

    public float bulletFrequency = 2f;  //cada cuánto se dispara cada bala
    private float lastShot = 0f;        //tiempo desde el último disparo

    private AudioSource audioSource;
    public AudioClip enemyHurtSound, enemyDeathSound, shootSound;
    public float minPitch = 1f;
    public float maxPitch = 1.43f;

    //--------------------------------- V. A W A K E ---------------------------------//

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1f;
    }

    //--------------------------------- V. S T A R T ---------------------------------//

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarOriginalScale = healthBar.localScale;
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
        if (SSBulletSpawn != null)
        {
            Vector3 bulletToPlayer = (player.transform.position - SSBulletSpawn.position).normalized;
            SS_Bullet bulletNew = Instantiate(SSBullet, SSBulletSpawn.position, SSBulletSpawn.rotation);
            bulletNew.Shooting(bulletToPlayer);

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(shootSound);
        }                                    
    }

    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damage;  //restamos el daño a la salud del enemigo
        UpdateHealthBar();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(enemyHurtSound);

        //aplicar el impulso hacia atrás
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)  //si la salud es menos que 0
        {
            if (Random.value <= 0.5f)  //50% de chances de que te de un Fragmento de Dios
            {
                Instantiate(shardPrefab, transform.position, Quaternion.identity);
            }

            audioSource.PlayOneShot(enemyDeathSound);

            Destroy(gameObject);  //muere
        }
    }
    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBar.localScale = new Vector3(healthBarOriginalScale.x * healthRatio, healthBarOriginalScale.y, healthBarOriginalScale.z);
    }
}
