using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedWater : MonoBehaviour
{
    public Movement movement;
    private GameObject player;

    public int damage = 1;
    [SerializeField]
    private int venomDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Player && !movement.isDashing)
        {
            Debug.Log("Im still poisoned");
            player = collision.gameObject;
            StartCoroutine(Poisoned(venomDuration, damage, player));
            collision.gameObject.GetComponent<Movement>().speed -= 2.1f;
            collision.gameObject.GetComponent<Movement>().dashCooldown += 10000000f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Player && !movement.isDashing)
        {
            collision.gameObject.GetComponent<Movement>().speed += 2.1f;
            collision.gameObject.GetComponent<Movement>().dashCooldown -= 10000000f;
            player = null;
        }
    }
    IEnumerator Poisoned(int poisonDuration, int poisonDamage, GameObject player)
    {
        Movement movement = player.GetComponent<Movement>();

        for (int i = poisonDuration; i > 0; i--)
        {
            int var1 = 0;
            Vector2 impactSource = transform.position;
            movement.TakeDamage(poisonDamage, impactSource, var1);
            yield return new WaitForSeconds(1.5f);
        }    

        Debug.Log("The poison is over...");
    }

   
}

public class Layers
{
    public const int Player = 6;
    public const int Enemies = 7;
    public const int Poison = 8;
    public const int HitboxPlayer = 9;
}