using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Reverse_Formation")]
public class Reverse_Formation : Formation_Base
{
    public Formation_Base formation;

    public override Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime)
    {
        base.SetUp(ref occuredBursts, ref ex_elapsedTime);
        int my_occuredBursts = occuredBursts.Peek();
        occuredBursts = formation.SetUp(ref occuredBursts, ref ex_elapsedTime);
        return occuredBursts;
    }

    public override Depth CalculateNesting(ref Stack<int> occuredBursts, Depth count)
    {
        count.nestDepth++;
        var my_occuredBursts = occuredBursts.Pop();
        count = formation.CalculateNesting(ref occuredBursts, count);
        occuredBursts.Push(my_occuredBursts);
        return count;
    }


    public override bool IsComplete(ref Stack<int> occurredBursts, float elapsedTime, float2 position)
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

        formation.UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling,
            position, ref ex_elapsedTime, !reversed);

        if (formation.IsComplete(ref occurredBursts, elapsedTime, position))
        {
            if (formation.IncrementElapsedTime()) 
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
