using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

namespace NPC
{
    [CreateAssetMenu(fileName = "NPC", menuName = "Characters/NPC")]
    public class Character : ScriptableObject
    {
        public CharacterBase character;
        public CharacterBase GetCharacter() => character;   
    }


    [Serializable]
    public class CharacterBase 
    {
        public string characterName;
        public ConversationGroup[] storyConversationsInOrder;
        public ConversationGroup[] termBasedOptionalConversations;

        public InteractionTree[] fillerDialogue; // Not by term
        public InteractionTree[] optionalConversations;

        public string GetTermSaveDataFileName() => characterName + "TermData";

        public int GetUnlockIndex(IndexReferenceType referenceType, int TermDate)
        {
            switch (referenceType)
            {
                case IndexReferenceType.mainIndex:
                    return GetUnlockIndexFromConversationPool(storyConversationsInOrder[TermDate].conversations);
                case IndexReferenceType.termOptionals:
                    return GetUnlockIndexFromConversationPool(termBasedOptionalConversations[TermDate].conversations);
                case IndexReferenceType.optionals:
                    return GetUnlockIndexFromConversationPool(optionalConversations);
                default:
                    return 0;
            }
        }

        private int GetUnlockIndexFromConversationPool(InteractionTree[] conversations)
        {
            return 0;
        }

    }
}
