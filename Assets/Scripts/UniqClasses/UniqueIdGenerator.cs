using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string uniqueId = UniqueIdGenerator.GenerateId();
        uniqueId = null;
    }

    public static string GenerateId()
    {
        return Guid.NewGuid().ToString();
    }
}
