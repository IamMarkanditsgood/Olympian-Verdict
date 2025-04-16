using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GodData
{
    [SerializeField] private GodTypes _god;
    [SerializeField] private Sprite _godSprite;
    [SerializeField] private string _godName;

    public GodTypes God => _god;
    public Sprite GodSprite => _godSprite;
    public string GodName => _godName;
}
