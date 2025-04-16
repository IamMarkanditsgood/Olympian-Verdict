using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    private void Start()
    {
        float noiseValue = GenerateNoise(1.23f, 4.56f);
        Debug.Log("Generated noise value: " + noiseValue);
    }
    public static float GenerateNoise(float x, float y)
    {
        return Mathf.PerlinNoise(x, y);
    }
}
