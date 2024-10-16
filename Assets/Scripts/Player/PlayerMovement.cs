using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public PlayerSlash playerSlash;
    public Transform groundCheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    private bool isGrounded;
    private bool canDash = true;
    
    private bool isInvincible = false;
    public bool IsInvincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }
    
    [Header("Movement Attributes")]
    public float jumpForce = 10f;
    public float dashSpeed = 50f;
    public float dashTime = 0.25f;
    public float speed = 12f;
    public float gravity = -9.8f;
    
    // New attribute for terminal velocity
    [Header("Terminal Velocity")]
    public float terminalVelocity = -50f; // Set terminal fall velocity

    private Vector3 velocity;



    void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // Reset velocity when grounded
            velocity.y = -2f;   
        }

        // Handle jumping
        if ((isGrounded || playerSlash.canJumpPostSlash) && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Apply movement
        controller.Move(move * speed * Time.deltaTime);
        
        // Handle dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash(move));
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply terminal fall velocity - Clamp velocity.y to prevent it from exceeding terminalVelocity
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);

        // Move player according to velocity
        controller.Move(velocity * Time.deltaTime);
    }
    
    public void removeFallVelocity()
    {
        velocity.y = -2f;
    }

    IEnumerator Dash(Vector3 move)
    {
        canDash = false;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            controller.Move(move * dashSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(2f);
        canDash = true;
    }
}
