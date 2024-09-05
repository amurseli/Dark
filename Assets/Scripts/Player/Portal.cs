using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destination;
    public GameObject player;
    
    private CharacterController characterController;

    private void Start()
    {
        // Get the CharacterController from the player
        characterController = player.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the player is the one entering the portal
        if (other.gameObject == player)
        {
            // Disable the CharacterController to prevent overrides
            characterController.enabled = false;

            // Set the player's new position at the destination
            Vector3 newPosition = destination.transform.position;
            newPosition.y -= 1.0f;  // Adjust Y offset if necessary
            player.transform.position = newPosition;

            Debug.Log("Teleported to: " + newPosition);

            // Re-enable the CharacterController after teleporting
            characterController.enabled = true;
        }
    }
}