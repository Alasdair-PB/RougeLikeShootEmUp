using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "SaveGameEvent", menuName = "DialogueEvents/SaveGameEvent")]
    public class SaveGameEvent : DialogueEventsScriptable
    {
        public SaveGameEvent_Base saveGameEvent;
        public override DialogueEvents GetDialogueEvents() => saveGameEvent;
        public override void OnEventCalled(Game game) => game.SaveGameState();
    }


    [Serializable]
    public class SaveGameEvent_Base : DialogueEvents
    {

    }
}
