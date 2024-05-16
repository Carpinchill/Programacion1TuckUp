using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyY : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public float health = 33f;          //salud del enemigo

    public Movement player;             //ref al player para obtener sus datos
    public float playerNear = 1f;       //cu�n cerca tiene que estar el jugador
        
    public EnY_Bullet EnYBullet;        //bullet prefab
    public Transform EnYBulletSpawn;    //spawn point


    //---------------------------------- A W A K E ----------------------------------//
    private void Awake()
    {
        
    }


    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        
    }

    //called ONCE PER FRAME
    void Update()
    {
        //si el jugador est� cerca, dispara
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < playerNear)
        {
            Shoot();
        }        
    }

    //---------------------------------- M E T H O D S ----------------------------------//

    //--- RECIBIR DA�O ---//
    public void GetDamage(float amount)
    {
        health -= amount;
        print("El honguito recibi� " + amount + " de da�o y su vida actual es de " + health);

        if (health <= 0)
        {
            print("El honguito se muri�");
            Destroy(gameObject);
        }
    }


    //--- DISPARAR ---//
    public void Shoot()
    {
        EnY_Bullet newBullet = Instantiate(EnYBullet, EnYBulletSpawn.position, transform.rotation);
    }
}
