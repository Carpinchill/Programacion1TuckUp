using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnX_Spawn : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//
    public EnemyX enemyX;           //prefab enemy

    public float health = 33f;      //salud spawn plant
    public float frequence = 5f;    //cada cuánto spawnea un enemigo             

    //---------------------------------- V. U P D A T E ----------------------------------//


    void Update()
    {
        //FALTA QUE SE SPAWNEEN SEGÚN public float frequence Y NO A CADA FRAME
        EnemyX newEnemyX = Instantiate(enemyX, transform.position, transform.rotation);
    }


    //---------------------------------- M E T H O D S ----------------------------------//

    //--- RECIBIR DAÑO ---//
    public void GetDamage(float amount)
    {
        health -= amount;
        print("El enemigo azul recibió " + amount + " de daño y su vida actual es de " + health);

        if (health <= 0)
        {
            print("El enemigo azul se murió");
            Destroy(gameObject);
        }
    }
}
