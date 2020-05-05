using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public enum TerrainType
    {
        Normal = 1,
        Difficult = 2,
        Impassable = 3
    }

    public enum MovementDirection
    {
        Up=1,
        Right=2,
        Down=3,
        Left=4
    }

    public enum UnitState
    {
        Idle=0,
        Selected=1,
        Moving=2,
        OutOfActions=3
    }
}
