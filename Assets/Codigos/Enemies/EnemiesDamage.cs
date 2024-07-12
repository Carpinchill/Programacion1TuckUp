using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDamage : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public GameObject[] enemies;
    public EnemyX EX_health;
    public EnemyY EY_health;
    public EnemyZ EZ_health;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //--- LLAMAR A LOS ENEMIGOS ---//
    void CallCommonMethod()
    {
        foreach (GameObject enemy in enemies)
        {
            EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
            if (enemyScript != null)
            {
                enemyScript.CommonMethod();
            }
        }
    }


    //--- RECIBIR ATAQUE DEL JUGADOR ---//   
    public virtual void TakeDamage(float damage, float knockbackForce, Vector3 knockbackDirection)
    {
        health -= damage;                                       //restamos el damage a la salud del enemigo
        Debug.Log("Enemy received " + damage + " damage.");     //print del damage

        if (TryGetComponent<Rigidbody2D>(out var rb))           //obtenemos el RigidBody del enemigo
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);    //le aplicamos el knockback
        }
        if (health <= 0)        //comprobamos vida, si se acabó
        {
            Die();              //muereee :(
        }
    }


    //--- MUEREE!!! ---//
    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("This enemy's gone to heaven.");
    }
}
