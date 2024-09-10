using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
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

    public List<Material> materials = new List<Material>();

    public virtual void GetHit(float dmg)
    {
        life -= dmg;
        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", hitColor);
        }

        StartCoroutine(FeedbackHit());
    }

    public IEnumerator FeedbackHit()
    {
        Debug.Log("FeedbackHit");
        yield return new WaitForSeconds(0.1f);
        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", baseColor);
        }
    }
}
