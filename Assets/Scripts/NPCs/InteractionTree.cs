using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "Tree", menuName = "Interactions/Tree")]
    [Serializable]
    public class InteractionTree : ScriptableObject
    {
        public InteractionTreeBase interactionTreeBase;
    }

    [Serializable]
    public class InteractionTreeBase
    {
        public Interaction interaction;
        public bool repeatDialogue; 
        public bool lockThisTreeOnComplete; 
        public InteractionTree followUpInteractionTree; 
        public bool endInteractiblityOnComplete;

        public InteractionTree[] conversationUnlocks;
    }
}