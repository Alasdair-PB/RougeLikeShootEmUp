using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ConversationGroup", menuName = "Interactions/ConversationGroup")]
public class ConversationGroup: ScriptableObject
{
    public InteractionTree[] conversations;
}
