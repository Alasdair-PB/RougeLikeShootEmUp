using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Multi_Formation")]
public class Multi_Formation : Formation_Base
{
    public Formation_Base[] formations;


    public override Stack<int> SetUp(Stack<int> occuredBursts)
    {
        base.SetUp(occuredBursts);
        int my_occuredBursts = occuredBursts.Peek();
        occuredBursts = formations[my_occuredBursts].SetUp(occuredBursts);
        //occuredBursts.Push(my_occuredBursts);
        return occuredBursts;
    }

    public override bool IsComplete(Stack<int> occurredBursts, float elapsedTime, float ex_elapsedTime, float2 position)
    {
        var isComplete = false;
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts >= formations.Length)
            isComplete = true;

        occurredBursts.Push(my_occuredBursts);
        return isComplete;
    }

    public override Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position, ref float ex_elapsedTime)
    {
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts >= formations.Length)
        {
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        formations[my_occuredBursts].UpdateFormation(layerMask, occurredBursts, elapsedTime, pooling, position, ref ex_elapsedTime);

        if (formations[my_occuredBursts].IsComplete(occurredBursts, elapsedTime, ex_elapsedTime, position))
        {
            ex_elapsedTime += elapsedTime;
            my_occuredBursts++;

            if (my_occuredBursts < formations.Length)
                formations[my_occuredBursts].SetUp(occurredBursts);
        }

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
