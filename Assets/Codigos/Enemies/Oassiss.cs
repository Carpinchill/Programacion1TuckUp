using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oassiss : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//
        
    public Movement player;                     //ref al script del jugador
    public Attack playerAttack;                 //ref al script del ataque del jugador
    public float playerNear = 9f;              //cuán cerca tiene que estar el jugador para ser detectado
    
    public Transform healthBar;                 //ref a la barra de vida
    public GameObject shardPrefab;              //ref al prefab de los shards
    public GameObject wallDestroy;
    public float maxHealth = 80;
    public float currentHealth;                 //salud actual
    private Vector3 healthBarOriginalScale;     //ref a la escala original de la barra de vida

    public Transform[] waypoints;               //array para almacenar waypoints
    int currentWaypoint = 0;                    //waypoint actual
    public float speed = 3f;                    //velocidad del enemigo

    public float lastH, lastV;
    private Vector3 lastPosition;
    private Animator animatorOA;

    public float damagePush = 10f;              //daño por colisión
    public float attackCooldown = 2f;           //tiempo hasta volver a atacar
    public float knockbackForceOA = 20f;        //retroceso aplicado POR ENEMIGO -> AL JUGADOR        
    private bool isKnockback = false;           //pregunta si si está retrocediendo

    public OA_Bullet OABullet;                  //ref al prefab de la bala
    public Transform OABulletSpawn;             //ref al transform del bullet spawn point
    public float bulletFrequency = 1f;          //cada cuánto se dispara cada bala
    private float lastShot = 0f;                //tiempo desde el último disparo

    private AudioSource audioSource;
    public AudioClip enemyHurtSound, enemyDeathSound, shootSound, wallDestroySound;    
    public float minPitch = 0.66f;
    public float maxPitch = 1.33f;    

    //--------------------------------- V. A W A K E ---------------------------------//
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.75f;
    }

    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>();           //obtenemos el ataque del jugador
        animatorOA = GetComponent<Animator>();
        lastPosition = transform.position;
        currentHealth = maxHealth;
        healthBarOriginalScale = healthBar.localScale;
    }


    //--------------------------------- L. U P D A T E ---------------------------------//

    private void LateUpdate()
    {
        animatorOA.SetFloat("LastH", lastH);
        animatorOA.SetFloat("LastV", lastV);
    }
    //--------------------------------- V. U P D A T E ---------------------------------//

    //--- RUTINA DE OASSISS ---//
    void Update()
    {
        if (isKnockback) return;    //si está en retroceso, que no haga nada más
        else if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)    //si el jugador está cerca
        {
            Push();       //que ataque
        }
        else              //si no, que patrulle
        {
            Patrol();
        }
        UpdateAnimatorOAParameters();
    }

    //--------------------------------- M E T H O D S ---------------------------------//

    //--- PATRULLAR ---//
    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)   //si el enemigo está muy cerca de un waypoint
        {
            currentWaypoint++;                               //el waypoint actual pasa a ser el siguiente

            if (currentWaypoint >= waypoints.Length)         //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0;                         //vuelve al primero
            }
        }

        //dirección del mov (hacia siguiente waypoint)
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        //cambio de posición
        transform.position = newPosition;
    }
        

    //--- DISPARAR ---//
    void Shoot()
    {
        if (OABulletSpawn != null)
        {
            Vector3 bulletToPlayer = (player.transform.position - OABulletSpawn.position).normalized;
            OA_Bullet bulletNew = Instantiate(OABullet, OABulletSpawn.position, OABulletSpawn.rotation);
            bulletNew.Shooting(bulletToPlayer);

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(shootSound);
        }
    }

    //--- EMPUJAR ---//
    void Push()
    {
        //pasamos los parametros al animator
        UpdateAnimatorOAParameters();

        //guardamos la posición del jugador
        Vector3 directionToPlayer = (player.gameObject.transform.position - transform.position).normalized;

        //mov HACIA el jugador
        transform.position += speed * Time.deltaTime * directionToPlayer;
    }

    void UpdateAnimatorOAParameters()
    {
        Vector3 movement = transform.position - lastPosition;

        if (movement != Vector3.zero)
        {
            //normalizamos el movimiento para obtener solo la dirección
            Vector3 normalizedMovement = movement.normalized;
            animatorOA.SetFloat("LastH", normalizedMovement.x);
            animatorOA.SetFloat("LastV", normalizedMovement.y);
        }

        //actualiza la última posición
        lastPosition = transform.position;
    }

    //--- COLISIONAMOS CON JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement player = collision.gameObject.GetComponentInParent<Movement>();
        if (player != null)
        {
            Vector3 collisionDirection = (transform.position - collision.transform.position).normalized; //guardamos la dirección (opuesta al punto de colisión)
            StartCoroutine(Knockback(collisionDirection)); //retrocedemos con la dirección que guardamos recién
        }
    }


    //--- RETROCESO ---//
    IEnumerator Knockback(Vector3 direction)
    {
        isKnockback = true;
        float knockbackTime = 0.7f; // duración del retroceso
        float timer = 0f;

        player.TakeDamage(damagePush, direction, knockbackForceOA);

        while (timer < knockbackTime)
        {
            transform.position += speed * Time.deltaTime * direction;
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;

        for (int i = 0; i < 2; i++)
        {
            Shoot();
            yield return new WaitForSeconds(bulletFrequency); //tiempo entre disparos
        }

        Push();
    }


    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damage;  // Restamos el daño a la salud del enemigo
        UpdateHealthBar();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(enemyHurtSound);

        // Aplicar el impulso hacia atrás
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)  // Si la salud es menos que 0
        {
            if (Random.value <= 0.5f)  // 50% de chances de que te de un Fragmento de Dios
            {
                Instantiate(shardPrefab, transform.position, Quaternion.identity);
            }

            wallDestroy.SetActive(false);
            audioSource.PlayOneShot(wallDestroySound);

            audioSource.PlayOneShot(enemyDeathSound);
            Destroy(gameObject);  // Muere
        }
    }
    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBar.localScale = new Vector3(healthBarOriginalScale.x * healthRatio, healthBarOriginalScale.y, healthBarOriginalScale.z);
    }
}
