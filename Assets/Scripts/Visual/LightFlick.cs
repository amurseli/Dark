using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlick : MonoBehaviour
{
    private Light lightComponent;
    private float x = 1f;
    private void Update()
    {
        lightComponent = GetComponent<Light>();
        x += 0.005f;
        lightComponent.range =  Mathf.Abs(Mathf.Sin(x) * 20)  + 10;
        Debug.Log(30 * ((Mathf.Sin(x) + 1) * 0.6f));
    }
}
