using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Loop_Formation")]
public class Loop_Formation : Formation_Base
{
    public Formation_Base formation;
    public int loopCount;
    public bool loopEndless; 

    public override Stack<int> SetUp(Stack<int> occuredBursts)
    {
        base.SetUp(occuredBursts);
        int my_occuredBursts = occuredBursts.Peek();
        occuredBursts = formation.SetUp(occuredBursts);
        return occuredBursts;
    }

    public override bool IsComplete(ref Stack<int> occurredBursts, float elapsedTime, float ex_elapsedTime, float2 position)
    {
        var my_occuredBursts = occurredBursts.Pop();
        if (my_occuredBursts >= loopCount && !loopEndless)
        {
            occurredBursts.Push(my_occuredBursts);
            return true;
        } 

        occurredBursts.Push(my_occuredBursts);
        return false;
    }

    public override bool IncrementElapsedTime() => false;

    public override Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position, ref float ex_elapsedTime)
    {
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts >= loopCount)
        {
            if (loopEndless)
            {
                occurredBursts.Clear();
                return SetUp(occurredBursts);
            }

            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        formation.UpdateFormation(layerMask, occurredBursts, elapsedTime, pooling, position, ref ex_elapsedTime);

        if (formation.IsComplete(ref occurredBursts, elapsedTime, ex_elapsedTime, position))
        {
            if (formation.IncrementElapsedTime())
                ex_elapsedTime = elapsedTime;

            my_occuredBursts++;

            if (my_occuredBursts < loopCount)
                formation.SetUp(occurredBursts);
        }

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
