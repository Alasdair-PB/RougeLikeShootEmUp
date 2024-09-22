using System;
using UnityEngine;

namespace Conditions
{
    [CreateAssetMenu(fileName = "ItemBasedCondition", menuName = "Conditions/ItemBasedCondition")]
    public class ItemCondition : ScriptableCondition
    {
        public ItemConditionBase condition;
        public override Condition GetCondition() => condition;
    }

    [Serializable]
    public class ItemConditionBase : Condition
    {
        public InventoryObject inventoryObject;
        public enum Evaluation{ Greater, Less, Equal, GreaterEqual, LessEqual };
        public Evaluation evaluation;
        public float comparedValue;
        public override bool IsUnlocked(Game game) 
        {
            var invObj = inventoryObject.inventoryObject;
            float myValue = game.GetSavedData<float>(invObj.stringName, invObj.saveFile);
            switch (evaluation)
            {
                case Evaluation.Greater:
                    return myValue > comparedValue;
                case Evaluation.Less:
                    return myValue < comparedValue;
                case Evaluation.Equal:
                    return myValue == comparedValue;
                case Evaluation.LessEqual:
                    return myValue <= comparedValue;
                case Evaluation.GreaterEqual:
                    return myValue >= comparedValue;
                default:
                    return false;
            }
        }    
    
    }
}
