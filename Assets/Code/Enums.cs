﻿using System.Collections;
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

    public enum UnitState
    {
        Idle=0,
        Selected=1,
        Moving=2,
        OutOfActions=3
    }
}
