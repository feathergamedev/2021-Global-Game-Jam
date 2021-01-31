using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageArgs", menuName = "ScriptableObject/StageArgs")]
public class StageArguments : ScriptableObject
{
    public Vector3 PlayerInitPos;

    public Potion Answer;

    public List<Potion>      potionCandidates;
    public List<EEffectType> effectTypeCandidates;
    public List<EEffectRate> effectRateCandidates;
}
