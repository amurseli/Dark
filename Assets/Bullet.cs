using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;

    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.getHit(1);
        }
        Destroy(gameObject);
    }
}
