using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float life = 10f;
    public float speed = 10f;
    public Color baseColor;
    public Color hitColor;

    private GameObject player;
    private List<Material> materials = new List<Material>(); 

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
        foreach (Transform child in transform)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                materials.Add(renderer.materials.First());
            }
        }

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
        if (player == null) return; // Exit if player is not found

        Vector3 vec = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        gameObject.GetComponent<Rigidbody>().MovePosition(vec);

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}