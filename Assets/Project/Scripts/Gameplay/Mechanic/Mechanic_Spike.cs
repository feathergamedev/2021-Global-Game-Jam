using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic_Spike : Mechanic
{
    public override void GetTriggered(GameObject target)
    {
        base.GetTriggered(target);

        //TODO: 發送事件，不要直接呼叫其他Script
        if (1 << target.layer == (int)EGameLayer.Player)
            target.GetComponent<PlayerController>().Die();
    }
}
