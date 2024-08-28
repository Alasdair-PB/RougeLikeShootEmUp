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
        //Debug.Log("Index: " + my_occuredBursts + ", from count: " + formations.Length);
        count = formations[my_occuredBursts].CalculateNesting(ref occuredBursts, count);
        occuredBursts.Push(my_occuredBursts);
        return count;
    }

    public override bool IsComplete(ref Stack<int> occurredBursts)
    {
        var my_occuredBursts = occurredBursts.Pop();
        if (my_occuredBursts > formations.Length) // May be greater than or equal!!!
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

        formations[my_occuredBursts].UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling, position, ref ex_elapsedTime, reversed);

        if (formations[my_occuredBursts].IsComplete(ref occurredBursts))
        {
            var my_ElaspedTime = ex_elapsedTime.Pop();

            if (formations[my_occuredBursts].IncrementElapsedTime())
                my_ElaspedTime = elapsedTime;

            // Shouldn't remove items on final index- burst increments after this hence -2
            if (my_occuredBursts <= formations.Length - 2)
            {
                Depth i = formations[my_occuredBursts].CalculateNesting(ref occurredBursts, new Depth() { nestDepth = 0, layerDepth = 0 });

                for (int j = i.nestDepth; j > 0; j--)
                {
                    occurredBursts.Pop();
                }
                for (int j = i.layerDepth; j > 0; j--)
                {
                    ex_elapsedTime.Pop();
                }
                ex_elapsedTime.Push(my_ElaspedTime);

                my_occuredBursts++;
                formations[my_occuredBursts].SetUp(ref occurredBursts, ref ex_elapsedTime);
            }
        }

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
