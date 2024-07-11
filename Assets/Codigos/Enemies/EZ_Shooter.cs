using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZ_Shooter : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public EZ_Bullet EZBullet;      //bullet prefab
    public Transform EZBulletSpawn;  //spawn point

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
        EZ_Bullet newBullet = Instantiate(EZBullet, EZBulletSpawn.position, transform.rotation);
    }
}
