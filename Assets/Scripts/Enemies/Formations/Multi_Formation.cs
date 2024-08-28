using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Multi_Formation")]
public class Multi_Formation : Formation_Base
{
    public Formation_Base[] formations;


    public override Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime)
    {
        base.SetUp(ref occuredBursts, ref ex_elapsedTime);
        int my_occuredBursts = occuredBursts.Pop();
        occuredBursts = formations[my_occuredBursts].SetUp(ref occuredBursts, ref ex_elapsedTime);
        occuredBursts.Push(my_occuredBursts);
        return occuredBursts;
    }

    public override Depth CalculateNesting(ref Stack<int> occuredBursts, Depth count)
    {
        count.nestDepth++;
        var my_occuredBursts = occuredBursts.Pop();
        count = formations[my_occuredBursts].CalculateNesting(ref occuredBursts, count);
        occuredBursts.Push(my_occuredBursts);
        return count;
    }


    public override bool IsComplete(ref Stack<int> occurredBursts, float elapsedTime, float2 position)
    {
        var my_occuredBursts = occurredBursts.Pop();
        if (my_occuredBursts >= formations.Length)
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

        if (my_occuredBursts >= formations.Length)
        {
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        //Debug.Log("Updating value: " + my_occuredBursts + ", with count: " + occurredBursts.Count);
        formations[my_occuredBursts].UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling, position, ref ex_elapsedTime, reversed);

        if (formations[my_occuredBursts].IsComplete(ref occurredBursts, elapsedTime, position))
        {
            if (formations[my_occuredBursts].IncrementElapsedTime())
            {
                ex_elapsedTime.Pop();
                ex_elapsedTime.Push(elapsedTime);
            }

            Depth i = formations[my_occuredBursts].CalculateNesting(ref occurredBursts, new Depth() { nestDepth = 0, layerDepth = 0 });

            for (int j = i.nestDepth; j > 0; j--) // Probably shouldn't be minus one, but need to get rid of multi counter being at max
            {
                var z = occurredBursts.Pop();
                Debug.Log("Removing Element " + z);
            }

            my_occuredBursts++;

            if (my_occuredBursts < formations.Length)
            {
                formations[my_occuredBursts].SetUp(ref occurredBursts, ref ex_elapsedTime);
            }
        }

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
