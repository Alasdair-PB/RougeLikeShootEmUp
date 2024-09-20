using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "ItemChange", menuName = "DialogueEvents/ItemChange")]
    public class ItemChange : DialogueEventsScriptable
    {
        public ItemChangeBase itemChange;
        public override DialogueEvents GetDialogueEvents() => itemChange;
        public override void OnEventCalled(Game game)
        {
            var myItemObject = itemChange.item.inventoryObject;
            string savedData = game.GetSavedData(myItemObject.stringName, myItemObject.saveFile, "0");
            float savedDataAsFloat = myItemObject.GetSavedDataAsFloat(savedData);

            var canModify = myItemObject.CheckCanModify(itemChange.valueChange, savedDataAsFloat);
            savedDataAsFloat += itemChange.valueChange;

            string savedDataAsString = myItemObject.GetSavedDataAsString(savedDataAsFloat);
            game.UpdateSavedValue(myItemObject.stringName, myItemObject.saveFile, savedDataAsString);
        }
    }

    [Serializable]
    public class ItemChangeBase : DialogueEvents
    {
        public InventoryObject item;
        public float valueChange; 
    }
}
