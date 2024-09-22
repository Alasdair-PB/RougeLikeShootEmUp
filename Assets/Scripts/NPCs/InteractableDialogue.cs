using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class InteractableDialogue : MonoBehaviour
    {
        [SerializeField] protected Game game;
        [SerializeField] protected InteractionTree[] possibleInteractions;
        [SerializeField] protected bool interactable;

        protected InteractionTree currentInteraction;

        Stack<InteractionTree> removalTreeStack = new Stack<InteractionTree>();
        public bool IsInteractable() => interactable;

        public void Start()
        {
            game = Game.Instance;
            SetNextInteraction();
        }

        protected void SetNextInteraction()
        {
            InteractionTree interaction = possibleInteractions[0];
            currentInteraction = interaction;
        }

        public void OnInteractionComplete()
        {
            var tree = currentInteraction.interactionTreeBase;

            if (tree.lockThisTreeOnComplete)
                removalTreeStack.Push(currentInteraction);

            if (tree.repeatDialogue)
                return;


            if (tree.endInteractiblityOnComplete)
            {
                interactable = false;
                currentInteraction = null;
                return;
            }
            else if (tree.followUpInteractionTree == null)
                SetNextInteraction();
            else
                currentInteraction = tree.followUpInteractionTree;

        }

        public Interaction GetInteraction() => currentInteraction.interactionTreeBase.interaction;
    }
}
