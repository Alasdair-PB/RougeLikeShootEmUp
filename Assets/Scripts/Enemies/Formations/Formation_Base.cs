using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public abstract class Scriptable_FormationBase : ScriptableObject
{
    public abstract Formation_Base GetFormation();
}


[Serializable]
public abstract class Formation_Base
{
    [SerializeField] public float2 positionOffset;

    public virtual Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime) {
        occuredBursts.Push(0);
        return occuredBursts;
    }

    public virtual Depth CalculateNesting(ref Stack<int> occuredBursts, Depth count) {
        count.nestDepth ++;
        return count;
}


    public struct Depth
    {
        public int nestDepth;
        public int layerDepth; 
    }
    public abstract bool IncrementElapsedTime();
    public abstract bool IsComplete(ref Stack<int> occurredBursts);
    public abstract Stack<int> UpdateFormation(LayerMask layerMask, ref Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position, ref Stack<float> ex_elapsedTime, bool reversed);
}


[Serializable]
public struct FormationData
{
    public Scriptable_FormationBase formation_Base;
    public float2 positionOffset;
    public bool reversed;
}