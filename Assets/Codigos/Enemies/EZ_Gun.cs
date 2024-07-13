using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZ_Gun : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//
    
    public EY_Bullet EZBullet;       //bullet prefab
    public Transform EZBulletSpawn;  //spawn point

    
    //---------------------------------- M E T H O D S ----------------------------------//

    //--- DISPARAR ---//
    public void Shoot()
    {
        EY_Bullet newBullet = Instantiate(EZBullet, EZBulletSpawn.position, transform.rotation);
        Vector3 bulletDirection = (transform.parent.position - transform.position).normalized;                //dirección (hacia el jugador)
        newBullet.Shooting(bulletDirection);                                                                  //le pasamos esa dirección a la bala
    }
}