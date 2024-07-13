using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZ_ : MonoBehaviour
{
    public float health = 100f;         //salud del enemigo
    public float speed = 15f;           //velocidad del enemigo
    public float damage = 10f;          //da�o al empujar

    public Transform[] waypoints;       //array para almacenar waypoints
    int currentWaypoint = 0;            //para ubicar en qu� waypoint estamos

    public Movement player;             //ref al player para obtener sus datos
    public float playerNear = 1f;       //cu�n cerca tiene que estar el jugador para que lo detecte
    public Attack playerAttack;         //ref al script del ataque del jugador

    public EZ_Gun[] EZGuns;             //array de spawn points de las balas
    public float attackCooldown = 1f;   //cooldown entre cada disparo
    public int maxShots = 3;            //cantidad m�xima de disparos por ataque

    public float knockbackForceEnemy = 2000f; //retroceso aplicado POR ENEMIGO -> AL JUGADOR

    private bool isKnockback = false;   //pregunta si est� retrocediendo
    private bool isAttacking = false;   //para saber si est� en fase de ataque
    //private int shotsFired = 0;         //cantidad de disparos realizados en el ataque actual

    //--------------------------------- A W A K E ---------------------------------//
    private void Awake()
    {
        EZGuns = GetComponentsInChildren<EZ_Gun>();
    }


    //--------------------------------- V. S T A R T ---------------------------------//

    void Start()
    {
        playerAttack = player.GetComponent<Attack>(); //obtenemos el ataque del jugador                
        StartCoroutine(AttackRoutine()); //comienza la rutina de ataques
    }


    //--------------------------------- V. U P D A T E ---------------------------------//

    void Update()
    {
        if (isKnockback || isAttacking) return; //si est� en retroceso o atacando, no hace nada m�s
        else if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear) //si el jugador est� cerca
        {
            StartCoroutine(AttackPlayer()); //inicia el ataque al jugador
        }
        else //si no, que patrulle
        {
            Patrol();
        }
    }

    //--------------------------------- M E T H O D S ---------------------------------//

    //--- PATRULLAR ---//
    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f) //si el enemigo est� muy cerca de un waypoint
        {
            currentWaypoint++; //el waypoint actual pasa a ser el siguiente
            if (currentWaypoint >= waypoints.Length) //si el siguiente es mayor o igual al total de waypoints
            {
                currentWaypoint = 0; //vuelve al primero
            }
        }

        //direcci�n del movimiento (hacia siguiente waypoint)
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        //cambio de posici�n
        transform.position = newPosition;
    }

    //--- ATACAR AL JUGADOR ---//
    IEnumerator AttackPlayer()
    {
        isAttacking = true;

        // Realizar tres disparos simult�neos desde los spawn points
        for (int i = 0; i < maxShots; i++)
        {
            foreach (EZ_Gun gun in EZGuns)
            {
                gun.Shoot(); //dispara desde cada spawn point
            }
            yield return new WaitForSeconds(attackCooldown); //espera antes del pr�ximo disparo
        }

        // Despu�s de los disparos, acercarse al jugador
        StartCoroutine(Knockback((player.gameObject.transform.position - transform.position).normalized));

        isAttacking = false;
    }

    //--- COLISIONAR CON EL JUGADOR ---//
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement player = collision.gameObject.GetComponentInParent<Movement>();
        if (player != null)
        {
            StartCoroutine(Knockback((transform.position - collision.transform.position).normalized));
            player.TakeDamage(damage, (transform.position - collision.transform.position).normalized, knockbackForceEnemy);
        }
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

    //--- RUTINA DE ATAQUE ---//
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (!isAttacking && !isKnockback && Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
            {
                yield return StartCoroutine(AttackPlayer());
            }
            yield return null;
        }
    }

    //--- RECIBIR DA�O ---//
    public void ReceiveDamage(float damage)
    {
        health -= damage; //restamos el da�o a la salud del enemigo

        if (health <= 0) //si la salud es menos que 0
        {
            Destroy(gameObject); //muere
            Debug.Log("This enemy's gone to hell");
        }
    }
}
