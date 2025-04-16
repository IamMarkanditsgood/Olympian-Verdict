using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class asdasdrrrr : MonoBehaviour
{
    public void Start()
    {
        asdasdad();
    }
    public async void asdasdad()
    {
        await Task.Delay(500); 
        Debug.Log("Data fetched from API simulator");
    }
}
