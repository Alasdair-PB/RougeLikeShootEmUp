using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Conditions
{
    public abstract class ScriptableCondition : ScriptableObject
    {
        public abstract Condition GetCondition();
    }

    [Serializable]
    public abstract class Condition 
    {
        public abstract bool IsUnlocked(Game game);
    }

}
