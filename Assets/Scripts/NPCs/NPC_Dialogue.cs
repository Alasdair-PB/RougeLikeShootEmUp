using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPC_Dialogue : InteractableDialogue
    {
        [SerializeField] private bool saveConversationHistory;
        [SerializeField] private Character character;
        private int termDate;
        private SerializableList<int> dialogueUnlocks; 

        Stack<InteractionTree> removalTreeStack = new Stack<InteractionTree>();

        public new void Start() 
        {
            game = Game.Instance;
            termDate = game.GetTermDate();

            if (!(character == null))
                dialogueUnlocks = game.GetSavedData<SerializableList<int>>(character.name, "");


            SetNextInteraction();
        }

        private void GetPossibleInteractions()
        {
            InteractionTree[] savedPossibleInteractions = new InteractionTree[0];
            possibleInteractions = savedPossibleInteractions;
        }

        private new void SetNextInteraction()
        {
            var index = 0; // dialogueUnlocks[0]
            var chara = character.GetCharacter();
            InteractionTree interaction = chara.storyConversationsInOrder[termDate].conversations[index];
            currentInteraction = interaction;
        }

        public new void OnInteractionComplete()
        {
            var tree = currentInteraction.interactionTreeBase;

            if (tree.lockThisTreeOnComplete)
                removalTreeStack.Push(currentInteraction);

            if (tree.repeatDialogue)
                return;


            if (tree.endInteractiblityOnComplete) {
                interactable = false;
                currentInteraction = null;
                return;
            } else if (tree.followUpInteractionTree == null)
                SetNextInteraction();
            else
                currentInteraction = tree.followUpInteractionTree;

        }
    }
}
