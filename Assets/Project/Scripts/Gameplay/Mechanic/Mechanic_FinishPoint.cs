using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic_FinishPoint : Mechanic
{
    public override void GetTriggered(GameObject target)
    {
        if (1 << target.layer == (int)EGameLayer.Player)
        {
            base.GetTriggered(target);

            StageManager.instance.DemoRoundFinished();

            ShutDown();
        }
    }
}
