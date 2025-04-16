using System;
using UnityEngine;

[Serializable]
public class GodAdvice
{
    [SerializeField] private GodTypes _god;
    [SerializeField] private string _godAdvice;

    public GodTypes God => _god;
    public string GodAdviceText => _godAdvice;
}
