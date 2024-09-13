using System;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "Tree", menuName = "Interactions/Tree")]
    public class InteractionTree : ScriptableObject
    {
        public InteractionTreeBase interactionTreeBase;
    }


    public struct GameSaveData
    {
        public int currentTerm;
        public int termAttemptCount;
        public int yearAttemptCount; // Not including term attempt count
    }

    // Collectible save data

    // Can be cleared on term progression?
    public struct InteractionTreeSaveData
    {
        public int interactionTreeId; // Maybe should be type InteractionTreeBase- depends on if the whole tree will be saved or just the object reference
        public bool completed, unlocked;
        public float priority; // Maybe shouldn't be here depends on systen
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