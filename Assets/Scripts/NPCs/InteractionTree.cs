using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "Tree", menuName = "Interactions/Tree")]
    public class InteractionTree : ScriptableObject
    {
        public InteractionTreeBase interactionTreeBase;
    }



    // Completed, unlocked, priority not included as this needs to be saved as user info

    [Serializable]
    public class InteractionTreeBase
    {
        public Interaction interaction;
        public bool repeatDialogue; // Can dialogue be repeated
        public bool removeOnScreenFade; // Is dialogue removed from options after scene transition
        public InteractionTree followUpInteractionTree; // Does this interaction open up a new InteractionTree afterwards
        public bool endInteractiblityOnComplete;
    }
}