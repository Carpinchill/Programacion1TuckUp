using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnZ_Shooter : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public EnZ_Bullet EnZBullet;      //bullet prefab
    public Transform EnZBulletSpawn;  //spawn point

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
        EnZ_Bullet newBullet = Instantiate(EnZBullet, EnZBulletSpawn.position, transform.rotation);
    }
}
