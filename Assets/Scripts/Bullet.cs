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

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        var player = other.gameObject.GetComponent<PlayerCombat>();
        if (enemy != null)
        {
            enemy.GetHit(1);
        }
        Debug.Log("Hitted an enemy!");

        if (player == null)
        {
            Destroy(gameObject);
        }
    }
}
