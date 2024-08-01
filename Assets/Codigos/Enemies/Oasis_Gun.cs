using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oasis_Gun : MonoBehaviour
{
    //---------------------------------- V A R I A B L E S ----------------------------------//

    public Movement player;             //ref al player (posición para takedamage

    public Oasis_Bullet OABullet;       //bullet prefab
    public Transform OABulletSpawn;     //spawn point

    private AudioSource audioSource;
    public AudioClip shootSound;
    public float minPitch = 0.80f;
    public float maxPitch = 1.25f;


    //--------------------------------- V. A W A K E ---------------------------------//

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }


    //---------------------------------- M E T H O D S ----------------------------------//

    //--- DISPARAR ---//
    void Shoot()
    {
        if (OABulletSpawn != null)
        {
            Vector3 bulletToPlayer = (player.transform.position - OABulletSpawn.position).normalized;
            Oasis_Bullet bulletNew = Instantiate(OABullet, OABulletSpawn.position, OABulletSpawn.rotation);
            bulletNew.Shooting(bulletToPlayer);

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(shootSound);
        }
    }
}