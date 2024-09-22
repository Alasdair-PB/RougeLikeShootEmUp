using Codice.CM.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPC_Dialogue : InteractableDialogue
    {
        [SerializeField] private bool saveConversationHistory;
        [SerializeField] private Character character;
        private int termDate;

        private enum IndexReferenceType { mainIndex, termOptionals, optionals };
        private IndexReferenceType indexType;
        private int interactionIndex;
        
        private NPC_GameData myGameData;

        public new void Start() 
        {
            game = Game.Instance;
            termDate = game.GetTermDate();

            if (!(character == null))
                myGameData = game.GetSavedData<NPC_GameData>(GetTermSaveDataFileName(), "");

            UpdateDataToCurrentTerm();
            SetNextInteraction();
        }

        private void UpdateDataToCurrentTerm()
        {
            // If the term date of the unlocked character interactions does not match the current term date then clear unlocked conversations
            if (!(myGameData.termDateAtLastSave == termDate))
            {
                var newGameData = myGameData;
                newGameData.termDateAtLastSave = termDate;
                newGameData.mainIndex = 0;
                newGameData.termOptional = 0;
                game.UpdateSavedValue<NPC_GameData>(GetTermSaveDataFileName(), "", newGameData);
            }
        }

        private string GetTermIndexSaveDataFileName() => character.name + "TermIndex";
        private string GetTermSaveDataFileName() => character.name + "TermData";
        private string GetOptionalSaveDataFileName() => character.name + "OptionalData";


        private void GetPossibleInteractions()
        {
            InteractionTree[] savedPossibleInteractions = new InteractionTree[0];
            possibleInteractions = savedPossibleInteractions;
        }

        private new void SetNextInteraction()
        {
            var interaction = GetPriorityInteractionTree(); 
            currentInteraction = interaction;
        }


        // Tell save data that this converation has occurred 
        private void RemoveGameData()
        {
            switch (indexType)
            {
                case IndexReferenceType.mainIndex:

                    var chara = character.GetCharacter();
                    var mainConversations = chara.storyConversationsInOrder[termDate].conversations;

                    if (myGameData.mainIndex >= mainConversations.Length - 1 )
                        myGameData.mainIndex = -1;
                    else if (!(myGameData.mainIndex == -1))
                        myGameData.mainIndex++;

                    break;
                case IndexReferenceType.termOptionals:
                    // Implement using binary shifts
                    break;
                case IndexReferenceType.optionals:
                    // Implement using binary shifts
                    break;
            }
        }

        public new void OnInteractionComplete()
        {
            var tree = currentInteraction.interactionTreeBase;

            if (tree.lockThisTreeOnComplete)
                RemoveGameData();

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

        public InteractionTree GetPriorityInteractionTree()
        {
            var chara = character.GetCharacter();
            var mainConversations = chara.storyConversationsInOrder[termDate].conversations;

            if (myGameData.mainIndex >= mainConversations.Length)
                myGameData.mainIndex = mainConversations.Length - 1;

            InteractionTree nextDialogue;

            if (!(myGameData.mainIndex == -1))
                nextDialogue = mainConversations[myGameData.mainIndex];
            else
                nextDialogue = chara.fillerDialogue[0];

            // Check nextStory scene conditions have been met

            interactionIndex = 0;
            for (int i = 0; i < chara.termBasedOptionalConversations.Length; i++)
            {
                // Check IsUnlocked if yes
                // Check if priority is higher than main story/ current nextInteraction
            }


            indexType = IndexReferenceType.mainIndex; // which list was used to get the index
            return nextDialogue;
        }

        public bool IsUnlocked(int index) => (1 << index) != 0;

    }
}
