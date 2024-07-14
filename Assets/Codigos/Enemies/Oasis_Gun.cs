using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oasis_Gun : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//
    
    public SS_Bullet SSBullet;       //bullet prefab
    public Transform SSBulletSpawn;  //spawn point

    
    //---------------------------------- M E T H O D S ----------------------------------//

    //--- DISPARAR ---//
    public void Shoot()
    {
        SS_Bullet newBullet = Instantiate(SSBullet, SSBulletSpawn.position, transform.rotation);
        Vector3 bulletDirection = (transform.parent.position - transform.position).normalized;                //dirección (hacia el jugador)
        newBullet.Shooting(bulletDirection);                                                                  //le pasamos esa dirección a la bala
    }
}