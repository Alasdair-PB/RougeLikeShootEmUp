using System;
using UnityEngine;

namespace Conditions
{
    [CreateAssetMenu(fileName = "Interaction", menuName = "Interactions/Interaction")]
    public class ItemCondition : ScriptableCondition
    {
        ItemConditionBase condition;
        public override Condition GetCondition() => condition;
    }

    [Serializable]
    public class ItemConditionBase : Condition
    {
        InventoryObject inventoryObject;
        public override bool IsUnlocked(Game game) => true;
    }
}
