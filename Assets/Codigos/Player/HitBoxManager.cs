using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public Attack attackScript;

    //--- LLAMAR AL MÉTODO RECEIVE DAMAGE DEL ENEMIGO ---//
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

        GlassKnight enemyX = other.gameObject.GetComponentInParent<GlassKnight>();
        if (enemyX != null)
        {
            enemyX.ReceiveDamage(attackScript.damage, knockbackDirection, attackScript.knockbackForce);
        }

        SouledShroom enemyY = other.gameObject.GetComponentInParent<SouledShroom>();
        if (enemyY != null)
        {
            enemyY.ReceiveDamage(attackScript.damage, knockbackDirection, attackScript.knockbackForce);
        }

        Oassiss enemyZ = other.gameObject.GetComponentInParent<Oassiss>();
        if (enemyZ != null)
        {
            enemyZ.ReceiveDamage(attackScript.damage, knockbackDirection, attackScript.knockbackForce);
        }
    }
}
