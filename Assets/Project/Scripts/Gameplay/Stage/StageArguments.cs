using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageArgs", menuName = "ScriptableObject/StageArgs")]
public class StageArguments : ScriptableObject
{
    public Potion Answer;

    public List<EEffectType> effectTypeCandidates;
    public List<EEffectRate> effectRateCandidates;
}
