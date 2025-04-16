using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnPositivePressed;
    public static event Action OnNegativePressed;

    public static void PositivePressed()
    {
        OnPositivePressed?.Invoke();
    }
    public static void NegativePressed()
    {
        OnNegativePressed?.Invoke();
    }
}
