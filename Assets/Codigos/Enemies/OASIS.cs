using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OASIS : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float currentHealth;         //salud del enemigo
    public float maxHealth = 80;
    public float speed = 15f;           //velocidad del enemigo    
    
    public Transform[] waypoints;       //array para almacenar waypoints
    int currentWaypoint = 0;            //para ubicar en qué waypoint estamos

    public Movement player;             //ref al player para obtener sus datos
    public float playerNear = 1f;       //cuán cerca tiene que estar el jugador para que lo detecte
    public Attack playerAttack;         //ref al script del ataque del jugador
    public GameObject breakableWall;

    private Oasis_Gun[] EZGuns;            //array de bullet spawns
    public int maxShots = 3;            //cantidad máxima de disparos por ataque
    //private int shotsFired = 0;         //cantidad de disparos realizados en el ataque actual
    public float shotCooldown = 1f;     //cooldown entre cada disparo

    public float pushDamage = 15f;      //daño al empujar
    public float pushForceEnemy = 20f;  //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    private bool isKnockback = false;   //pregunta si está retrocediendo (ENEMIGO <- JUGADOR)
    private bool isAttacking = false;   //pregunta si está atacando

    public float lastH, lastV;
    private Animator animatorOasis;
    private Vector3 lastPosition;

    private AudioSource audioSource;
    public AudioClip enemyHurtSound, enemyDeathSound;
    public float minPitch = 0.80f;
    public float maxPitch = 1.25f;


    //--------------------------------- A W A K E ---------------------------------//
    private void Awake()
    {
        EZGuns = GetComponentsInChildren<Oasis_Gun>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }


    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>();           //obtenemos el ataque del jugador
        animatorOasis = GetComponent<Animator>();
        lastPosition = transform.position;
        currentHealth = maxHealth;
    }
    //--------------------------------- L. U P D A T E ---------------------------------//

    private void LateUpdate()
    {
        animatorOasis.SetFloat("LastH", lastH);
        animatorOasis.SetFloat("LastV", lastV);
    }

    //--------------------------------- V. U P D A T E ---------------------------------//

    //--- RUTINA DE OASIS ---//
    void Update()
    {
        if (isKnockback || isAttacking) return;             //si está en retroceso o atacando, que no haga nada más
        else if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)       //si el jugador está cerca
        {
            StartCoroutine(AttackPlayer());                 //que ataque al jugador
        }
        else                                                //si no, que patrulle
        {
            Patrol();
        }
        UpdateAnimatorOasisParameters();
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
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        UpdateAnimatorOasisParameters();
        //dispara X veces desde los 2 spawn points simultáneamente
        for (int i = 0; i < maxShots; i++)
        {
            foreach (Oasis_Gun gun in EZGuns)
            {
                gun.Shoot();                                    //dispara desde cada spawn point
            }
            yield return new WaitForSeconds(shotCooldown);      //espera antes del próximo disparo
        }

        Push();
    }
    void UpdateAnimatorOasisParameters()
    {
        Vector3 movement = transform.position - lastPosition;

        if (movement != Vector3.zero)
        {
            //normalizamos el movimiento para obtener solo la dirección
            Vector3 normalizedMovement = movement.normalized;
            animatorOasis.SetFloat("LastH", normalizedMovement.x);
            animatorOasis.SetFloat("LastV", normalizedMovement.y);
        }

        //actualiza la última posición
        lastPosition = transform.position;
    }

    void Push()
    {
        //guardamos la posición del jugador
        Vector3 directionToPlayer = (player.gameObject.transform.position - transform.position).normalized;

        //mov HACIA el jugador
        transform.position += speed * Time.deltaTime * directionToPlayer;        
    }

    //--- COLISIONAR CON EL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement player = collision.gameObject.GetComponentInParent<Movement>();
        if (player != null)
        {
            StartCoroutine(Knockback((transform.position - collision.transform.position).normalized));
            player.TakeDamage(pushDamage, (transform.position - collision.transform.position).normalized, pushForceEnemy);
        }        

        AttackPlayer();
    }


    //--- RETROCESO ---//
    IEnumerator Knockback(Vector3 direction)
    {
        isKnockback = true;
        float knockbackTime = 0.7f; // duración del retroceso
        float timer = 0f;

        while (timer < knockbackTime)
        {
            transform.position += speed * Time.deltaTime * direction;
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;
    }


    //--- RECIBIR DAÑO ---//
    public void ReceiveDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damage;  //restamos el daño a la salud del enemigo

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(enemyHurtSound);

        //aplicar el impulso hacia atrás
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)  //si la salud es menos que 0
        {
            breakableWall.SetActive(false);
            audioSource.PlayOneShot(enemyDeathSound);
            //sonido de pared rota
            Destroy(gameObject);  //muere
           
        }
    }
}
