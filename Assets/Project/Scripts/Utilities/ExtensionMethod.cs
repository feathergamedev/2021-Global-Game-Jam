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
        return $"{((int)rate).ToString("+0;-#")}%";
    }

    public static string Label(this EEffectType type)
    {
        var labelMsg = "";

        switch (type)
        {
            case EEffectType.Damage:
                labelMsg = "Damage";
                break;
            case EEffectType.AttackFrequency:
                labelMsg = "AttackFreq";
                break;
            case EEffectType.MoveSpeed:
                labelMsg = "MoveSpeed";
                break;
            case EEffectType.MaxHP:
                labelMsg = "MaxHP";
                break;                
        }

        return labelMsg;
    }

    public static Vector3 ScreenToWorldPos(this Camera cam, Vector3 vector)
    {
        var result = Camera.main.ScreenToWorldPoint(vector);
        result.z = 0f;

        return result;
    }
}
