using System;
using UnityEngine;


namespace NPC
{
    public abstract class DialogueEvents { }

    public abstract class DialogueEventsScriptable : ScriptableObject
    {
        public abstract DialogueEvents GetDialogueEvents();

        public abstract void OnEventCalled(Game game);
    }

}
