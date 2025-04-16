using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateManager
{
    public int CalculateFactorial(int number)
    {
        if (number <= 1)
            return 1;
        return number * CalculateFactorial(number - 1);
    }
    public List<Vector3> SimulateParticles(int particleCount, int steps)
    {
        List<Vector3> positions = new List<Vector3>();
        System.Random random = new System.Random();

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 position = Vector3.zero;

            for (int step = 0; step < steps; step++)
            {
                position += new Vector3(
                    (float)(random.NextDouble() - 0.5),
                    (float)(random.NextDouble() - 0.5),
                    (float)(random.NextDouble() - 0.5)
                );
            }

            positions.Add(position);
        }

        return positions;
    }
}
