using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static float Value(this EEffectRate rate)
    {
        return (int)rate / 100f;
    }

    public static string Label(this EEffectRate rate)
    {
        return $"{(int)rate}%";
    }
}
