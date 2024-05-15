using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZ_bullet : MonoBehaviour
{
    public float speed = 5f;

    public float lifetime = 2f;

    public EnemyZ enemyZ;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
