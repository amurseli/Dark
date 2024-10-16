using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100; 
    
    private PlayerMovement _movement;
    
    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter(Collision collision)
    {  
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            receiveDamage(enemy.daño); 
        }
    }

    public void receiveDamage(int damage)
    {
        if (_movement.IsInvincible)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log("Player received " + damage + " damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died!");
        }
    }
}