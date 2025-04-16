using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEncryptor : MonoBehaviour
{
    private void Start()
    {
        string encrypted = SimpleEncryptor.Encrypt("Hello, World!", "key123");
        Debug.Log("Encrypted string: " + encrypted);
    }
    public static string Encrypt(string input, string key)
    {
        char[] keyArr = key.ToCharArray();
        char[] inputArr = input.ToCharArray();
        for (int i = 0; i < inputArr.Length; i++)
        {
            inputArr[i] ^= keyArr[i % keyArr.Length];
        }
        return new string(inputArr);
    }
}
