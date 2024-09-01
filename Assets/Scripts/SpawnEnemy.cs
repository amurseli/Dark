using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpawnEnemy : MonoBehaviour
{
    public float spawnRadius = 200f;


    public GameObject basicEnemy;

    private void Update()
    {
        StartCoroutine(Spawn(basicEnemy));
    }

    IEnumerator Spawn(GameObject basicEnemy)
    {
        yield return new WaitForSeconds(10f);
        //Instantiate(basicEnemy, )
        
    }


}
