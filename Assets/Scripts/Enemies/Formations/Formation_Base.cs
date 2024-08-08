using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public abstract class Formation_Base : ScriptableObject
{
    public Stack<int> SetUp(Stack<int> occuredBursts) {
        occuredBursts.Push(0);
        return occuredBursts;
    }

    public abstract bool IsComplete(Stack<int> occurredBursts, float elapsedTime, float2 position);
    public abstract Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position);
}
