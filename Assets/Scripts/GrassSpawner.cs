using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
     public Terrain terrain;
    public GameObject[] grassPrefabs;  // Array de diferentes tipos de pasto
    public int textureWidth = 512;
    public int textureHeight = 512;
    public float scale = 10f;
    public float threshold = 0.5f;  // Umbral general para permitir el spawn

    // Rango de probabilidades para cada tipo de pasto
    public float[] grassTypeThresholds = new float[] { 0.33f, 0.66f };  // Por defecto para 3 tipos de pasto

    void Start()
    {
        if (terrain == null || grassPrefabs.Length == 0)
        {
            Debug.LogError("Terreno o prefabs de pasto no asignados");
            return;
        }

        // Generar la textura de ruido Perlin
        Texture2D noiseTexture = GeneratePerlinNoiseTexture();

        TerrainData terrainData = terrain.terrainData;
        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;
        Vector3 terrainPosition = terrain.transform.position;

        // Iterar sobre la textura en función de su tamaño
        for (int x = 0; x < textureWidth; x++)
        {
            for (int z = 0; z < textureHeight; z++)
            {
                // Obtener el color del pixel (x, z) en la textura
                Color noiseColor = noiseTexture.GetPixel(x, z);
                float spawnProbability = noiseColor.grayscale;

                // Solo instanciar si el valor de gris es mayor o igual al umbral
                if (spawnProbability >= threshold)
                {
                    // Determinar el tipo de pasto según el valor del ruido
                    GameObject selectedGrass = SelectGrassType(spawnProbability);

                    if (selectedGrass != null)
                    {
                        // Convertir la posición del píxel en coordenadas del terreno
                        float terrainX = Mathf.Lerp(terrainPosition.x, terrainPosition.x + terrainWidth, (float)x / textureWidth);
                        float terrainZ = Mathf.Lerp(terrainPosition.z, terrainPosition.z + terrainLength, (float)z / textureHeight);

                        // Obtener la altura del terreno en esa posición
                        float height = terrain.SampleHeight(new Vector3(terrainX, 0, terrainZ)) + terrainPosition.y;

                        // Crear la posición donde se instanciará el prefab
                        Vector3 spawnPosition = new Vector3(terrainX, height, terrainZ);

                        // Instanciar el prefab seleccionado
                        Instantiate(selectedGrass, spawnPosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    // Función para seleccionar el tipo de pasto basado en el valor de ruido
    GameObject SelectGrassType(float noiseValue)
    {
        // Comparar el valor del ruido con los thresholds de tipos de pasto
        if (noiseValue < grassTypeThresholds[0])
        {
            return grassPrefabs[0];  // Tipo A
        }
        else if (noiseValue < grassTypeThresholds[1])
        {
            return grassPrefabs[1];  // Tipo B
        }
        else
        {
            return grassPrefabs[2];  // Tipo C
        }
    }

    // Función para generar la textura de ruido Perlin
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
