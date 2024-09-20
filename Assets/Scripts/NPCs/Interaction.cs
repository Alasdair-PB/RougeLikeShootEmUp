using System;
using UnityEngine;
using UnityEngine.Events;
using Conditions;


namespace NPC
{
    [CreateAssetMenu(fileName = "Interaction", menuName = "Interactions/Interaction")]
    [Serializable]
    public class Interaction : ScriptableObject
    {
        public InteractionBase interaction;
    }

    [Serializable]
    public struct OptionalInteraction
    {
        public string option; // As displayed in text box
        public Interaction overrideInteraction; // Can be null
        public DialogueEventsScriptable[] actionEvent;
        public ScriptableCondition unlockCondition;
    }

    [Serializable] 
    public class InteractionBase
    {
        public string dialogue;
        public int characterID, emotionID, backgroundID;
        public float textSpeed = 1, textSize = 1;
        public Color textColor = Color.black;
        public bool endsTextBox;

        public OptionalInteraction[] options;
        public Interaction nextInteraction;
        public DialogueEventsScriptable[] onComplete;
    }
}
