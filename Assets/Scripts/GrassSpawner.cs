using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [Header("Terreno y Prefabs")]
    public Terrain terrain;
    public GameObject[] grassPrefabs;

    [Header("Textura de Ruido")]
    public int textureWidth = 512;
    public int textureHeight = 512;
    [Range(1f, 20f)]  
    public float scale = 10f;
    [Range(0f, 1f)]  
    public float threshold = 0.5f;

    [Header("Probabilidades de Tipos de Pasto")]
    [Tooltip("Umbrales para determinar el tipo de pasto basado en el ruido")]
    public float[] grassTypeThresholds = new float[] { 0.33f, 0.66f };

    [Header("Parámetros de Tamaño del Pasto")]
    [Range(0.5f, 2f)]  
    public float minHeightMultiplier = 0.8f;
    [Range(0.5f, 2f)]  
    public float maxHeightMultiplier = 1.2f;

    [Range(0.5f, 2f)]  
    public float minWidthMultiplier = 0.8f;
    [Range(0.5f, 2f)]  
    public float maxWidthMultiplier = 1.2f;

    private GameObject grassParent;  

    void Start()
    {
        if (terrain == null || grassPrefabs.Length == 0)
        {
            Debug.LogError("Terreno o prefabs de pasto no asignados");
            return;
        }

        // Crear el GameObject vacío "pasto"
        grassParent = new GameObject("pasto");

        Texture2D noiseTexture = GeneratePerlinNoiseTexture();

        TerrainData terrainData = terrain.terrainData;
        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;
        Vector3 terrainPosition = terrain.transform.position;

        for (int x = 0; x < textureWidth; x++)
        {
            for (int z = 0; z < textureHeight; z++)
            {
                Color noiseColor = noiseTexture.GetPixel(x, z);
                float spawnProbability = noiseColor.grayscale;

                if (spawnProbability >= threshold)
                {
                    GameObject selectedGrass = SelectGrassType(spawnProbability);

                    if (selectedGrass != null)
                    {
                        float terrainX = Mathf.Lerp(terrainPosition.x, terrainPosition.x + terrainWidth, (float)x / textureWidth);
                        float terrainZ = Mathf.Lerp(terrainPosition.z, terrainPosition.z + terrainLength, (float)z / textureHeight);

                        float height = terrain.SampleHeight(new Vector3(terrainX, 0, terrainZ)) + terrainPosition.y;

                        Vector3 spawnPosition = new Vector3(terrainX, height + 1, terrainZ);

                        GameObject grassInstance = Instantiate(selectedGrass, spawnPosition, Quaternion.identity);

                        // Ajustar altura y ancho aleatorios
                        float randomHeight = GetRandomHeight();
                        float randomWidth = Random.Range(minWidthMultiplier, maxWidthMultiplier);
                        grassInstance.transform.localScale = new Vector3(randomWidth, randomHeight, randomWidth);

                        // Asignar el pasto como hijo del GameObject "pasto"
                        grassInstance.transform.parent = grassParent.transform;
                    }
                }
            }
        }
    }

    GameObject SelectGrassType(float noiseValue)
    {
        if (noiseValue < grassTypeThresholds[0])
        {
            return grassPrefabs[0];
        }
        else if (noiseValue < grassTypeThresholds[1])
        {
            return grassPrefabs[1]; 
        }
        else
        {
            return grassPrefabs[2]; 
        }
    }

    float GetRandomHeight()
    {
        float randomValue = Random.value; 
        float heightMultiplier = Mathf.Lerp(minHeightMultiplier, maxHeightMultiplier, randomValue * randomValue); 
        return heightMultiplier;
    }

    public Texture2D GeneratePerlinNoiseTexture()
    {
        Texture2D noiseTexture = new Texture2D(textureWidth, textureHeight);

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float xCoord = (float)x / textureWidth * scale;
                float yCoord = (float)y / textureHeight * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                noiseTexture.SetPixel(x, y, new Color(sample, sample, sample));
            }
        }

        noiseTexture.Apply();
        return noiseTexture;
    }
}
