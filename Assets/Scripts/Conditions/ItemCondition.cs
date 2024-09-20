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
            var myValue = game.GetSavedData(invObj.stringName, invObj.saveFile, "0");
            var myValueAsFloat = invObj.GetSavedDataAsFloat(myValue);
            switch (evaluation)
            {
                case Evaluation.Greater:
                    return myValueAsFloat > comparedValue;
                case Evaluation.Less:
                    return myValueAsFloat < comparedValue;
                case Evaluation.Equal:
                    return myValueAsFloat == comparedValue;
                case Evaluation.LessEqual:
                    return myValueAsFloat <= comparedValue;
                case Evaluation.GreaterEqual:
                    return myValueAsFloat >= comparedValue;
                default:
                    return false;
            }
        }    
    
    }
}
