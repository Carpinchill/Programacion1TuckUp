using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    //---------------------------------- V A R I A B L E S ----------------------------------------------------------------------------

    public GameObject[] Hitboxes;

    public Movement movement;

    [SerializeField]
    private float comboResetTime = 1.3f;

    [SerializeField]
    private float attackLifeSpan = 0.3f;

    [SerializeField]
    private float attackCooldown = 0.7f;

    private int currentAttack = 0;

    private float lastAttackTime;


    //---------------------------------- V. S T A R T ----------------------------------------------------------------------------
    void Start() // Desactivar todas las hitboxes al inicio
    {
        foreach (GameObject HitboxCombo in Hitboxes)
        {
            HitboxCombo.SetActive(false);
        }
    }


    //---------------------------------- V. U P D A T E ----------------------------------------------------------------------------

    void Update() // Verificar si ha pasado el tiempo límite para mantener el combo
    {
        if (Time.time >= lastAttackTime + comboResetTime)
        {
            currentAttack = 0;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown && !movement.isDashing)
        {
            PerformNextAttack();
        }
    }
    
    //---------------------------------- M E T O  ----------------------------------------------------------------------------

    void PerformNextAttack()  // Desactivar la hitbox del ataque anterior y empezar la corutina
    {
        if (currentAttack > 0)
        {
            Hitboxes[currentAttack - 1].SetActive(false);           
        }

        movement.enabled = false;

        StartCoroutine(NextAttack(Hitboxes[currentAttack], attackLifeSpan));

        Hitboxes[currentAttack].SetActive(true);

        lastAttackTime = Time.time;

        currentAttack = (currentAttack + 1) % Hitboxes.Length;
    }

    IEnumerator NextAttack (GameObject hitbox, float duration)
    {    
        yield return new WaitForSeconds(duration); 
        hitbox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        movement.enabled = true;
    }
}

