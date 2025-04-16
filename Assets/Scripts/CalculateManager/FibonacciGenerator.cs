using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibonacciGenerator : MonoBehaviour
{
    public List<int> GenerateFibonacci(int count)
    {
        List<int> fibonacci = new List<int> { 0, 1 };

        for (int i = 2; i < count; i++)
        {
            fibonacci.Add(fibonacci[i - 1] + fibonacci[i - 2]);
        }

        return fibonacci;
    }
}
