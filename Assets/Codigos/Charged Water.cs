using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedWater : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == Layers.Player)
        {
            collision.gameObject.GetComponent<Movement>().TakeDamage(damage);
        } 
    }
}


public class Layers
{
    public const int Player = 6;
}