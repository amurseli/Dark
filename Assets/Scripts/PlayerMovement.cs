using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private bool canDash = true;
    
    
    [Header("Movement Attributes")]
    public float jumpForce = 20f;
    public float dashSpeed = 50f;
    public float dashTime = 0.25f;
    public float speed = 12f;
    public float gravity = -9.8f;

    private Vector3 velocity;
    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;   
        }
        
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y += Mathf.Sqrt(jumpForce * -1 * gravity);
        }

        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * (speed * Time.deltaTime));
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash(move));
            Debug.Log("Dashed");
            canDash = false;
            StartCoroutine(DashCooldown());
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash(Vector3 move)
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            Debug.Log("Shitholemotherfuckaaa");
            controller.Move(move * (dashSpeed * Time.deltaTime));
            yield return null;
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(2f);
        canDash = true;
    }
}
