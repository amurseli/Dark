using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experienceBearer : MonoBehaviour
{
    public int experienceAmount = 1;
    public float detectionRadius = 5f; 
    public LayerMask playerLayer;      
    public float speed = 15f;
    private GameObject player;
    private bool playerFound = false;
    private experienceManager expManager;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        expManager = player.GetComponentInChildren<experienceManager>();
    }

    private void Update()
    {
        if (!playerFound)
        {
            if (PlayerInRange())
            {
                playerFound = true;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private bool PlayerInRange()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("PLAYER FOUND");
                return true; 
            }
        }
        return false; 
    }

    private void MoveTowardsPlayer()
    {
        Debug.Log("MOVING TOWARDS PLAYER");
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Dibuja una esfera en la escena para visualizar el área de detección
    }

    private void OnTriggerEnter(Collider other)
    {
        expManager.addExperience(experienceAmount);
        Destroy(gameObject);
    }
}
