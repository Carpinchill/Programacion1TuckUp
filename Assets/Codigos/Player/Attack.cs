using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attack : MonoBehaviour
{

    //---------------------------------- V A R I A B L E S ----------------------------------------------------------------------------

    public GameObject[] Hitboxes;
    public GameObject HitboxesRotation;
    public Movement movement;

    public float damage = 12f;
    public float knockbackForce = 5f;

    [SerializeField]
    private float comboResetTime = 1.3f;
    [SerializeField]
    private float attackLifeSpan = 0.3f;
    [SerializeField]
    private float attackCooldown = 0.7f;
    public float attackStaminaCost = 1f;    

    private int currentAttack = 0;
    private float lastAttackTime;


    //---------------------------------- V. S T A R T ----------------------------------------------------------------------------
    void Start()//desactivar todas las hitboxes al inicio
    {

        foreach (GameObject HitboxCombo in Hitboxes)
        {
            HitboxCombo.SetActive(false);
        }
    }

    //---------------------------------- V. U P D A T E ----------------------------------------------------------------------------

    void Update() //verificar si ha pasado el tiempo límite para mantener el combo
    {
        if (Time.time >= lastAttackTime + comboResetTime)
        {
            currentAttack = 0;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown && !movement.isDashing)
        {
            if (movement.currentStamina >= attackStaminaCost)
            {
                PerformNextAttack();
            }
        }

        float lastH = movement.lastH;
        float lastV = movement.lastV;

        //definir el ángulo de rotación basado en LastH y LastV
        float angle = 0f;
        if (lastH == -1)
        {
            angle = 90f; //izquierda
        }
        else if (lastH == 1)
        {
            angle = 270f; //aerecha
        }
        else if (lastV == -1)
        {
            angle = 180f; //abajo
        }
        else if (lastV == 1)
        {
            angle = 0f; //arriba
        }

        //aplicar la rotación al GameObject hitboxes
        HitboxesRotation.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    //---------------------------------- M E T O  ----------------------------------------------------------------------------

    void PerformNextAttack()  //desactiva la hitbox del ataque anterior y empieza la corutina
    {
        if (currentAttack > 0)
        {
            Hitboxes[currentAttack - 1].SetActive(false);           
        }

        movement.speed = 0;                                     //desactiva el movimiento y empieza la corutina

        movement.dashCooldown = 1000000;

        StartCoroutine(NextAttack(Hitboxes[currentAttack], attackLifeSpan));

        Hitboxes[currentAttack].SetActive(true);

        lastAttackTime = Time.time;

        currentAttack = (currentAttack + 1) % Hitboxes.Length;

        movement.ConsumeStamina(attackStaminaCost);
    }

    IEnumerator NextAttack (GameObject hitbox, float duration)
    {    
        yield return new WaitForSeconds(duration); 
        hitbox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        movement.speed = 5f;
        movement.dashCooldown = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyX x = collision.gameObject.GetComponentInParent<EnemyX>();
        if (x != null)
        {
            x.TakeDamage(damage, knockbackForce, (collision.transform.position - transform.position).normalized);
        }
    }
}

