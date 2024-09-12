using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPC_Dialogue : MonoBehaviour
    {
        [SerializeField] private InteractionTree[] possibleInteractions;
        [SerializeField] private bool interactable;

        private InteractionTree currentInteraction;
        Stack<InteractionTree> removalTreeStack = new Stack<InteractionTree>();

        public bool IsInteractable() => interactable;

        public void Awake()
        {
            SetNextInteraction();
        }

        private void OnDisable()
        {
            // Remove removalStack from future options
        }

        // Going to be a complicated save file retrieval data things with scriptable object references
        private void GetPossibleInteractions()
        {
            InteractionTree[] savedPossibleInteractions = new InteractionTree[0];
            possibleInteractions = savedPossibleInteractions;
        }

        private void SetNextInteraction()
        {
            InteractionTree interaction = possibleInteractions[0];
            currentInteraction = interaction;
        }

        public void OnInteractionComplete()
        {
            var tree = currentInteraction.interactionTreeBase;
            // More needed here

            if (tree.removeOnScreenFade)
                removalTreeStack.Push(currentInteraction);

            if (tree.repeatDialogue)
                return;


            if (tree.endInteractiblityOnComplete) { 
                currentInteraction = null;
                return;
            } else if (tree.followUpInteractionTree == null)
                SetNextInteraction();
            else
                currentInteraction = tree.followUpInteractionTree;

        }

        public Interaction GetInteraction() => currentInteraction.interactionTreeBase.interaction;
    }
}
