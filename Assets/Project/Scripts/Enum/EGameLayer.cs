using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameLayer
{
    EveryThing    = -1,
    Nothing       = 0,
    Default       = 1,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Water         = 1 << 4,
    UI            = 1 << 5,

    Player = 1 << 8,
    Platform = 1 << 9,
    Mechanic = 1 << 10,
    Enemy = 1 << 11,
}
