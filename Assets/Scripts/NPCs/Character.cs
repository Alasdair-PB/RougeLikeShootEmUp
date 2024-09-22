using System;
using UnityEngine;
using System.Collections.Generic;

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

    }
}
