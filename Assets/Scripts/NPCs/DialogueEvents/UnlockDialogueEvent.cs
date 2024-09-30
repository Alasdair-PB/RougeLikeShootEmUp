using System;
using UnityEngine;




////
/// This won't even work!!! Gamedata is not updated in NPC_Dialogue so any changes will be overwritten when NPC_Dialogue saves over the previous Gamedata
///

namespace NPC
{
    [CreateAssetMenu(fileName = "UnlockDialogueEvent", menuName = "DialogueEvents/UnlockDialogueEvent")]
    public class UnlockDialogueEvent : DialogueEventsScriptable
    {
        public UnlockDialogueEvent_Base unlockGameEvent;
        public Interaction interaction;
        public IndexReferenceType interacationGroup; 
        Character character;

        public override DialogueEvents GetDialogueEvents() => unlockGameEvent;
        public override void OnEventCalled(Game game)
        {
            var chara = character.GetCharacter();
            int indexValue = chara.GetUnlockIndex(IndexReferenceType.mainIndex, game.GetTermDate());

            //game.GetSavedData<int>(chara.GetTermSaveDataFileName(), "", );
            game.UpdateSavedValue<int>(chara.GetTermSaveDataFileName(), "", 1);
        }
    }




    [Serializable]
    public class UnlockDialogueEvent_Base : DialogueEvents
    {

    }
}
