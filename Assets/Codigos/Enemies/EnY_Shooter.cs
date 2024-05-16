using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnY_Shooter : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public EnY_Bullet EnYBullet;       //bullet prefab
    public Transform EnYBulletSpawn;   //spawn point

    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        
    }

    //called ONCE PER FRAME
    void Update()
    {
        
    }

    //---------------------------------- M E T H O D S ----------------------------------//

    //--- DISPARAR ---//
    public void Shoot()
    {
        EnY_Bullet newBullet = Instantiate(EnYBullet, EnYBulletSpawn.position, transform.rotation);
    }
}
