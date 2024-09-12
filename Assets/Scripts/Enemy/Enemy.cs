using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : EnemyBase
{

    private GameObject player;

    private Vector3 targetPosition
    {
        get => player.transform.position + Vector3.up;
    }
    private float orgDrag;
    
    private List<Vector3> waypoints = new List<Vector3>();
    private Vector3 lastPosition;
    private bool isFloaterEnemy = false;
    private bool wiggleExists;
    private float stuckTimer = -1;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach (Transform child in transform)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                materials.Add(renderer.materials.First());
            }
        }
        orgDrag = rb.drag;
        
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure a GameObject with the 'Player' tag exists in the scene.");
        }
    }

    private void Update()
    {

        if (player == null) return;

        if (!isFloaterEnemy)
        {
            HandleRegularMovement();
            
        }
        else
        {
            HandleFloaterMovement();
        }

        if (life <= 0)
        {
            die();
        }
    }

    protected override void die()
    {
        UISingleton.Instance.addScore();
        Destroy(gameObject);
    }
    
    void HandleFloaterMovement()
    {
        var targetDirection = (targetPosition - transform.position).normalized;
        if (!Physics.SphereCast(new Ray(transform.position, targetDirection), 0.5f,
                Vector3.Distance(transform.position, targetPosition), LayerMask.GetMask("Ground") ) &&
            !Physics.CheckSphere(transform.position + targetDirection, 0.5f,  LayerMask.GetMask("Ground")))
        {
            waypoints.Clear();
            var floatingPosition = new Vector3(targetPosition.x, targetPosition.y + 20, targetPosition.z);
            waypoints.Add(floatingPosition);
            
        }
        
        if(waypoints.Count > 0 && Vector3.Distance(waypoints.Last(), targetPosition) > 1f)
        {
            var floatingPosition = new Vector3(targetPosition.x, targetPosition.y + 200, targetPosition.z);
            waypoints.Add(floatingPosition);
        }

        if (waypoints.Count == 0) return;
        
        if(Vector3.Distance(transform.position, waypoints.Last()) < 2f)
        {
            waypoints.RemoveAt(0);
            wiggleExists = false;
        }
    }

    void HandleRegularMovement()
    {
        var targetDirection = (targetPosition - transform.position).normalized;
        if (!Physics.SphereCast(new Ray(transform.position, targetDirection), 0.5f,
                Vector3.Distance(transform.position, targetPosition), LayerMask.GetMask("Ground") ) &&
            !Physics.CheckSphere(transform.position + targetDirection, 0.5f,  LayerMask.GetMask("Ground")))
        {
            waypoints.Clear();
            waypoints.Add(targetPosition);
            
        }
        
        if(waypoints.Count > 0 && Vector3.Distance(waypoints.Last(), targetPosition) > 1f)
        {
            waypoints.Add(targetPosition);
        }

        if (waypoints.Count == 0) return;
        
        if(Vector3.Distance(transform.position, waypoints.Last()) < 2f)
        {
            waypoints.RemoveAt(0);
            wiggleExists = false;
        }
    }

    private void FixedUpdate()
    {
        if (waypoints.Count == 0)
        {
            rb.drag = -1f; 
            return;
        }
        else
        {
            rb.drag = orgDrag;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(waypoints[0] - rb.position), rotationSpeed * Time.fixedDeltaTime));
            
        }
        
        rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
        
        if (rb.velocity.magnitude > velocity)
        {
            rb.velocity = rb.velocity.normalized * velocity;
        }
        
        var distanceTravelled = Vector3.Distance(lastPosition, rb.position);
        lastPosition = rb.position;

        if (distanceTravelled < Time.fixedDeltaTime)
        {
            if (stuckTimer < 0) stuckTimer = Time.time;
            
            if (Time.time > stuckTimer + 1f)
            {
                var randomInCircle = UnityEngine.Random.insideUnitSphere * 4f;
                var wigglePosition = rb.position + new Vector3(randomInCircle.x, randomInCircle.y, randomInCircle.z);
                if (!wiggleExists)
                {
                    wiggleExists = true;
                    waypoints.Insert(0, wigglePosition);
                }
                else
                {
                    waypoints[0] = wigglePosition;
                }

                stuckTimer = -1;
            }
            
            
        }
        
        
    }
}