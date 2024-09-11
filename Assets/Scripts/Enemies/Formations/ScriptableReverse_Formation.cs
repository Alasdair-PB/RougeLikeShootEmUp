using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Reverse", menuName = "Formations/Reverse")]

public class ScriptableReverse_Formation : Scriptable_FormationBase
{
    public Reverse_Formation formation;
    public override Formation_Base GetFormation() => formation;
}


[Serializable]
public class Reverse_Formation : Formation_Base
{
    public Scriptable_FormationBase formation;

    public override Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime)
    {
        base.SetUp(ref occuredBursts, ref ex_elapsedTime);
        int my_occuredBursts = occuredBursts.Peek();
        occuredBursts = formation.GetFormation().SetUp(ref occuredBursts, ref ex_elapsedTime);
        return occuredBursts;
    }

    public override Depth CalculateNesting(ref Stack<int> occuredBursts, Depth count)
    {
        count.nestDepth++;
        var my_occuredBursts = occuredBursts.Pop();
        count = formation.GetFormation().CalculateNesting(ref occuredBursts, count);
        occuredBursts.Push(my_occuredBursts);
        return count;
    }


    public override bool IsComplete(ref Stack<int> occurredBursts)
    {
        var my_occuredBursts = occurredBursts.Pop();
        if (my_occuredBursts >= 1)
        {
            occurredBursts.Push(my_occuredBursts);
            return true;
        }

        occurredBursts.Push(my_occuredBursts);
        return false;
    }

    public override bool IncrementElapsedTime() => false;

    public override Stack<int> UpdateFormation(LayerMask layerMask, ref Stack<int> occurredBursts, float elapsedTime,
        GlobalPooling pooling, float2 position, ref Stack<float> ex_elapsedTime, bool reversed)
    {
        position += positionOffset;
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts >= 1)
        {
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        formation.GetFormation().UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling,
            position, ref ex_elapsedTime, !reversed);

        if (formation.GetFormation().IsComplete(ref occurredBursts))
        {
            if (formation.GetFormation().IncrementElapsedTime()) 
            {
                ex_elapsedTime.Pop();
                ex_elapsedTime.Push(elapsedTime);
            }
            my_occuredBursts++;
        }

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
