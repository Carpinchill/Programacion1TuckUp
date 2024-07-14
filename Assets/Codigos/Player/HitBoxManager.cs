using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public Attack attackScript;

    //--- LLAMAR AL MÉTODO RECEIVE DAMAGE DEL ENEMIGO ---//
    private void OnTriggerEnter2D(Collider2D other)
    {
        GlassKnight enemyX = other.gameObject.GetComponentInParent<GlassKnight>();
        if (enemyX != null)
        {
            enemyX.ReceiveDamage(attackScript.damage);
        }

        SouledShroom enemyY = other.gameObject.GetComponentInParent<SouledShroom>();
        if (enemyY != null)
        {
            enemyY.ReceiveDamage(attackScript.damage);
        }

        OASIS enemyZ = other.gameObject.GetComponentInParent<OASIS>();
        if (enemyZ != null)
        {
            enemyZ.ReceiveDamage(attackScript.damage);
        }
    }
}
