using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------------------------------------------------
    public Attack attack;

    public float speed = 5f;

    private Rigidbody2D rb2d;

    public int maxHealth = 20;

    public float currentHealth;

    public GameObject DashEffect;

    public bool isDashing = false;

    [SerializeField]
    private float dashDuration;

    [SerializeField]
    private float dashForce;

    public float dashCooldown = 1.2f;

    private float lastDashTime;

    float lastH, lastV;

    private Animator animator;


    //---------------------------------- A W A K E ----------------------------------------------------------------------------

    //cuando inicia pone el primer frame, setea la salud al maximo, desactiva el efecto del dash y las hitbox y agarra el rigidbody
    private void Awake()
    {
        DashEffect.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    //---------------------------------- F. U P D A T E ----------------------------------------------------------------------------


    private void FixedUpdate()
    {
        Move();
    }

    //---------------------------------- L. U P D A T E ----------------------------------------------------------------------------


    //setea las animaciones correspondientes
    private void LateUpdate()
    {
        animator.SetFloat("LastH", lastH);
        animator.SetFloat("LastV", lastV);
    }

    //---------------------------------- M E T O D O S ----------------------------------------------------------------------------



    //con esto se mueve y guarda la ultima direccion hacia donde fue para el dash y las animaciones
    void Move()
    {
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");

        rb2d.MovePosition((Vector2)transform.position + speed * Time.fixedDeltaTime * new Vector2(axisH, axisV).normalized);

        if(axisH != 0 || axisV != 0)
        { 
            lastH = axisH;
            lastV = axisV;
        }
    }

    //chequea la ultima direccion y la pasa a la corutina para saber hacia donde dashear
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown) 
        {
            float dashH = lastH != 0 ? lastH : Input.GetAxisRaw("Horizontal");
            float dashV = lastV != 0 ? lastV : Input.GetAxisRaw("Vertical");
            Vector2 dashDirection = new Vector2(dashH, dashV).normalized;
            StartCoroutine(Dash(dashDirection));
        }
    }

    

    //este es el codigo para el dash
    IEnumerator Dash(Vector2 direction)
    {
        lastDashTime = Time.time;
        isDashing = true;
        DashEffect.SetActive(true);
        rb2d.velocity = direction * dashForce;
        enabled = false;
        attack.enabled = false;
       
        yield return new WaitForSeconds(dashDuration);

        attack.enabled = true;
        enabled = true;
        isDashing = false;
        DashEffect.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (isDashing == false) 
        { 
        currentHealth -= damage;

        Debug.Log("I received " + damage + " damage " + "My health is " + currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        //aca va un sistema que baje la barra de vida
    }

    private void Die()
    {
        enabled = false;
        attack.enabled = false;
    }
}