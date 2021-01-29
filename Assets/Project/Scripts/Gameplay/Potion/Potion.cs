using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "ScriptableObject/Potion")]
[System.Serializable]
public class Potion : ScriptableObject
{
    public EEffectType Type;
    public EEffectRate Rate;

    public Potion(EEffectType type, EEffectRate rate)
    {
        this.Type = type;
        this.Rate = rate;
    }
}