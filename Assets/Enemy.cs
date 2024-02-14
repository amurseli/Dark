using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float life = 10;
    public float speed = 10f;
    private GameObject player;
    public void getHit(float dmg)
    {
        life -= dmg;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    private void Update()
    {
        Vector3 vec = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        gameObject.GetComponent<Rigidbody>().MovePosition(vec);
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
