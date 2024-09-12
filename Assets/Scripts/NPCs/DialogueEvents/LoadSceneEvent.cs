using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "loadSceneEvent", menuName = "DialogueEvents/loadSceneEvent")]
    public class LoadSceneEvent: DialogueEventsScriptable
    {
        public LoadSceneEventBase loadSceneEvent;
        public override DialogueEvents GetDialogueEvents() => loadSceneEvent;
        public override void OnEventCalled(Game game) => game.LoadNewScene(loadSceneEvent.sceneName);
    }


    [Serializable]
    public class LoadSceneEventBase : DialogueEvents
    {
        public string sceneName;
    }
}
