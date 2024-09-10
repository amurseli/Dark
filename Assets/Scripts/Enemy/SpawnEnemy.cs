using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    public float spawnRadius = 200f;
    public float spawnRate = 5f;


    public GameObject basicEnemy;


    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;

            if (spawnPosition.y < 0)
            {
                spawnPosition.y = -spawnPosition.y;
            }


            Instantiate(basicEnemy, spawnPosition, Quaternion.identity);
        }

    }

}
