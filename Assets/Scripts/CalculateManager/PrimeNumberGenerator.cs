using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimeNumberGenerator : MonoBehaviour
{
    public List<int> GeneratePrimes(int count)
    {
        List<int> primes = new List<int>();
        int number = 2;

        while (primes.Count < count)
        {
            bool isPrime = true;
            for (int i = 2; i <= Mathf.Sqrt(number); i++)
            {
                if (number % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            if (isPrime)
                primes.Add(number);

            number++;
        }

        return primes;
    }
}
