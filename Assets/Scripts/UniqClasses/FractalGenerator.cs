using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string fractalData = FractalGenerator.GenerateFractal(100);
        // ћожна десь записати або вивести в консоль
        Debug.Log(fractalData);
    }

    public static string GenerateFractal(int iterations)
    {
        var result = new System.Text.StringBuilder();
        double x = 0.5, y = 0.5;
        for (int i = 0; i < iterations; i++)
        {
            x = Math.Sin(y * y - x);
            y = Math.Cos(x * x - y);
            result.Append($"{x:F3},{y:F3};");
        }
        return result.ToString();
    }
}
