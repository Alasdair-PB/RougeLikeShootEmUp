using System;
using UnityEngine;
using UnityEngine.Events;


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
        public UnityEvent actionEvent;
    }

    [Serializable] 
    public class InteractionBase
    {
        public string dialogue;
        public int characterID, emotionID, backgroundID;
        public float textSpeed, textSize;
        public Color textColor;
        public bool endsTextBox;

        public OptionalInteraction[] options;
        public Interaction nextInteraction;
        public UnityEvent onComplete;
    }
}
