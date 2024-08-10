using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public abstract class Formation_Base : ScriptableObject
{
    public virtual Stack<int> SetUp(Stack<int> occuredBursts) {
        occuredBursts.Push(0);
        return occuredBursts;
    }

    public abstract bool IncrementElapsedTime();
    public abstract bool IsComplete(ref Stack<int> occurredBursts, float elapsedTime, float ex_elapsedTime, float2 position);
    public abstract Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position, ref float ex_elapsedTime);
}