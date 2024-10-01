using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassSpawner : MonoBehaviour
{

    public Mesh mesh;
    public Material material;
    
    private ComputeBuffer argsBuffer;

    void Start()
    {
        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        args[0] = (uint)mesh.GetIndexCount(0);
        args[1] = (uint) 10000;
        args[2] = (uint)mesh.GetIndexStart(0);
        args[3] = (uint)mesh.GetBaseVertex(0);
        argsBuffer.SetData(args);
    }


}
