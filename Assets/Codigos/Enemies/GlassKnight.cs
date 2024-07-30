using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GlassKnight : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float currentHealth;                  //salud del enemigo
    public float maxHealth = 40;
    public float damage = 6f;                   //daño que hace el enemigo
    public float speed = 9f;                    //velocidad del enemigo

    public Movement player;                     //ref al script del jugador
    public Attack playerAttack;                 //ref al script del ataque del jugador   
    public GameObject shardPrefab;              //ref al prefab de los shards

    public Transform[] waypoints;               //array para almacenar waypoints
    int currentWaypoint = 0;                    //waypoint actual
    public float playerNear = 5f;               //cuán cerca tiene que estar el jugador para ser detectado
        
    public float attackCooldown = 2f;           //tiempo hasta volver a atacar
    public float knockbackForceEnemy = 120f;    //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    private bool isKnockback = false;           //pregunta si si está retrocediendo

    public float lastH, lastV;
    private Animator animatorGK;
    private Vector3 lastPosition;

    private AudioSource audioSource;

    public AudioClip enemyHurtSound, enemyDeathSound;
    public float minPitch = 0.80f;
    public float maxPitch = 1.25f;


    //--------------------------------- V. A W A K E ---------------------------------//
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>();           //obtenemos el ataque del jugador
        animatorGK = GetComponent<Animator>();
        lastPosition = transform.position;
        currentHealth = maxHealth;
    }


    //--------------------------------- L. U P D A T E ---------------------------------//

    private void LateUpdate()
    {
        animatorGK.SetFloat("LastH", lastH);
        animatorGK.SetFloat("LastV", lastV);
    }
    //--------------------------------- V. U P D A T E ---------------------------------//

    //--- RUTINA DEL ENEMIGO X ---//
    void Update()
    {        
        if (isKnockback) return;    //si está en retroceso, que no haga nada más
        else if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)    //si el jugador está cerca
        {
            Attack();       //que ataque
        }
        else                //si no, que patrulle
        {
            Patrol();
        }
        UpdateAnimatorGKParameters();
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


    //--- ATACAR ---//
    void Attack()
    {
        //pasamos los parametros al animator
        UpdateAnimatorGKParameters();

        //guardamos la posición del jugador
        Vector3 directionToPlayer = (player.gameObject.transform.position - transform.position).normalized;

        //mov HACIA el jugador
        transform.position += speed * Time.deltaTime * directionToPlayer;
    }
    void UpdateAnimatorGKParameters()
    {
        Vector3 movement = transform.position - lastPosition;

        if (movement != Vector3.zero)
        {
            //normalizamos el movimiento para obtener solo la dirección
            Vector3 normalizedMovement = movement.normalized;
            animatorGK.SetFloat("LastH", normalizedMovement.x);
            animatorGK.SetFloat("LastV", normalizedMovement.y);
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


        player.TakeDamage(damage, direction, knockbackForceEnemy);

        while (timer < knockbackTime)
        {
            transform.position += speed * Time.deltaTime * direction;
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;

        //decidimos qué hacer después del retroceso
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)    //si el jugador está cerca
        {
            Attack(); //que ataque
        }
        else      //si NO está cerca
        {
            currentWaypoint = 0;    //el waypoint actual vuelve a ser el primero
            Patrol();               //que patrulle
        }
    }


    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damage;  // Restamos el daño a la salud del enemigo

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

            audioSource.PlayOneShot(enemyDeathSound);

            Destroy(gameObject);  // Muere
        }
    }
}
