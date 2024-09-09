using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float rotationSpeed = 5f;
    public float acceleration = 100f;
    public float velocity = 10f;
    public float life = 10f;
    public float damage = 10f;
    
    [Header("Colors")]
    public Color baseColor;
    public Color hitColor;

    private GameObject player;

    private Vector3 targetPosition
    {
        get => player.transform.position + Vector3.up;
    }

    private List<Material> materials = new List<Material>();
    private float orgDrag;
    
    private List<Vector3> waypoints = new List<Vector3>();
    private Vector3 lastPosition;
    private bool wiggleExists;
    private float stuckTimer = -1;
    private Rigidbody rb;
    

    public void GetHit(float dmg)
    {
        life -= dmg;
        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", hitColor);
        }

        StartCoroutine(FeedbackHit());
    }

    IEnumerator FeedbackHit()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", baseColor);
        }
    }

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
        //Physics.IgnoreCollision(GetComponent<SphereCollider>(), player.GetComponent<CapsuleCollider>());
        orgDrag = rb.drag;
        
        //targetPosition = player.transform.position;

        Debug.Log($"Number of children: {transform.childCount}");
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
        //if (player == null) return; // Exit if player is not found

        //Vector3 vec = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        //gameObject.GetComponent<Rigidbody>().MovePosition(vec);

        if (player == null) return;
        
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

        if (life <= 0)
        {
            Destroy(gameObject);
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
                Debug.Log("Stuck!");
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