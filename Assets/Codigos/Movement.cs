using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Rigidbody2D rb2d;

    [SerializeField]
    public float maxHealth = 7f;

    [SerializeField]
    private float currentHealth;

    public GameObject DashEffect;

    private bool isDashing = false;

    [SerializeField]
    public float dashDuration = 0.25f;

    [SerializeField]
    public float dashForce = 15f;

    private Vector2 dashDirection;

    float lastH, lastV;

    private Animator animator;


    //cuando inicia pone el primer frame, setea la salud al maximo, desactiva el efecto del dash y agarra el rigidbody
    private void Awake()
    {
        DashEffect.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //setea las animaciones correspondientes
    private void LateUpdate()
    {
        animator.SetFloat("LastH", lastH);
        animator.SetFloat("LastV", lastV);
    }


    //con esto se mueve y guarda la ultima direccion hacia donde fue para el dash y las animaciones
    void Move()
    {
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");

        rb2d.MovePosition((Vector2)transform.position + new Vector2(axisH, axisV).normalized * speed * Time.fixedDeltaTime);

        if(axisH != 0 || axisV != 0)
        { 
            lastH = axisH;
            lastV = axisV;
        }
    }

    //chequea la ultima direccion y la pasa a la corutina para saber hacia donde dashear
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing) 
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
        isDashing = true;
        DashEffect.SetActive(true);
        rb2d.velocity = direction * dashForce;
        enabled = false;
       
        yield return new WaitForSeconds(dashDuration);

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
            return;
        }
        //aca va un sistema que baje la barra de vida
    }

    private void Die()
    {
       
    }
}