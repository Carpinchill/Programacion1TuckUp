using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OASIS : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public float health = 100f;         //salud del enemigo
    public float speed = 15f;           //velocidad del enemigo    
    
    public Transform[] waypoints;       //array para almacenar waypoints
    int currentWaypoint = 0;            //para ubicar en qu� waypoint estamos

    public Movement player;             //ref al player para obtener sus datos
    public float playerNear = 1f;       //cu�n cerca tiene que estar el jugador para que lo detecte
    public Attack playerAttack;         //ref al script del ataque del jugador

    private Oasis_Gun[] EZGuns;            //array de bullet spawns
    public int maxShots = 3;            //cantidad m�xima de disparos por ataque
    //private int shotsFired = 0;         //cantidad de disparos realizados en el ataque actual
    public float shotCooldown = 1f;     //cooldown entre cada disparo

    public float pushDamage = 15f;      //da�o al empujar
    public float pushForceEnemy = 20f;  //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    private bool isKnockback = false;   //pregunta si est� retrocediendo (ENEMIGO <- JUGADOR)
    private bool isAttacking = false;   //pregunta si est� atacando

    public float lastH, lastV;
    private Animator animatorOasis;
    private Vector3 lastPosition;

    private AudioSource audioSource;
    public AudioClip enemyHurtSound, enemyDeathSound;


    //--------------------------------- A W A K E ---------------------------------//
    private void Awake()
    {
        EZGuns = GetComponentsInChildren<Oasis_Gun>();
    }


    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>();           //obtenemos el ataque del jugador
        animatorOasis = GetComponent<Animator>();
        lastPosition = transform.position;
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
        if (isKnockback || isAttacking) return;             //si est� en retroceso o atacando, que no haga nada m�s
        else if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)       //si el jugador est� cerca
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
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)   //si el enemigo est� muy cerca de un waypoint
        {
            currentWaypoint++;                               //el waypoint actual pasa a ser el siguiente
            if (currentWaypoint >= waypoints.Length)         //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0;                         //vuelve al primero
            }
        }

        //direcci�n del mov (hacia siguiente waypoint)
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        //cambio de posici�n
        transform.position = newPosition;
    }


    //--- ATACAR ---//
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        UpdateAnimatorOasisParameters();
        //dispara X veces desde los 2 spawn points simult�neamente
        for (int i = 0; i < maxShots; i++)
        {
            foreach (Oasis_Gun gun in EZGuns)
            {
                gun.Shoot();                                    //dispara desde cada spawn point
            }
            yield return new WaitForSeconds(shotCooldown);      //espera antes del pr�ximo disparo
        }

        Push();
    }
    void UpdateAnimatorOasisParameters()
    {
        Vector3 movement = transform.position - lastPosition;

        if (movement != Vector3.zero)
        {
            //normalizamos el movimiento para obtener solo la direcci�n
            Vector3 normalizedMovement = movement.normalized;
            animatorOasis.SetFloat("LastH", normalizedMovement.x);
            animatorOasis.SetFloat("LastV", normalizedMovement.y);
        }

        //actualiza la �ltima posici�n
        lastPosition = transform.position;
    }

    void Push()
    {
        //guardamos la posici�n del jugador
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
        float knockbackTime = 0.7f; // duraci�n del retroceso
        float timer = 0f;

        while (timer < knockbackTime)
        {
            transform.position += speed * Time.deltaTime * direction;
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;
    }


    //--- RECIBIR DA�O ---//
    public void ReceiveDamage(float damage)
    {
        health -= damage;           //restamos el da�o a la salud del jugador

        audioSource.PlayOneShot(enemyHurtSound);

        if (health <= 0)            //si la salud es menos que 0
        {
            audioSource.PlayOneShot(enemyDeathSound);
            Destroy(gameObject);    //muere
            Debug.Log("This enemy's gone to hell");
        }
    }
}