using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EY_Shooter : MonoBehaviour
{
    //--------------------------------- V A R I A B L E S ---------------------------------//

    public EY_Bullet EYBullet;        //bullet prefab
    public Transform EYBulletSpawn;   //spawn point

    //called BEFORE FIRST FRAME UPDATE
    void Start()
    {
        
    }

    //called ONCE PER FRAME
    void Update()
    {
        
    }

    //--------------------------------- M E T H O D S ---------------------------------//

    //--- DISPARAR ---//
    public void Shoot()
    {
        EY_Bullet newBullet = Instantiate(EYBullet, EYBulletSpawn.position, transform.rotation);
    }
}
