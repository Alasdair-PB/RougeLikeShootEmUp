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
            float savedData = game.GetSavedData<float>(myItemObject.stringName, myItemObject.saveFile);

            var canModify = myItemObject.CheckCanModify(itemChange.valueChange, savedData);
            savedData += itemChange.valueChange;

            game.UpdateSavedValue(myItemObject.stringName, myItemObject.saveFile, savedData);
        }
    }

    [Serializable]
    public class ItemChangeBase : DialogueEvents
    {
        public InventoryObject item;
        public float valueChange; 
    }
}
