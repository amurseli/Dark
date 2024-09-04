using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [Header("Attributes")]
    public float rotationSpeed = 1;
    public enum Direction
    {
        X,
        Y,
        Z
    }
    public Direction rotationDirection;
    // Update is called once per frame
    void Update()
    {
        switch (rotationDirection)
        {
            case Direction.X:
                transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
                break;
            case Direction.Y:
                transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
                break;
            case Direction.Z:
                transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
                break;
        }
    }
}
