using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System;



[CreateAssetMenu(fileName = "Multi", menuName = "Formations/Multi")]

public class ScriptableMulti_Formation : Scriptable_FormationBase
{
    public Multi_Formation formation;
    public override Formation_Base GetFormation() => formation;
}


[Serializable]
public class Multi_Formation : Formation_Base
{
    public FormationData[] formations;

    public override Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime)
    {
        base.SetUp(ref occuredBursts, ref ex_elapsedTime);
        int my_occuredBursts = occuredBursts.Pop();
        occuredBursts = formations[my_occuredBursts].formation_Base.GetFormation().SetUp(ref occuredBursts, ref ex_elapsedTime);
        occuredBursts.Push(my_occuredBursts);
        return occuredBursts;
    }

    public override Depth CalculateNesting(ref Stack<int> occuredBursts, Depth count)
    {
        count.nestDepth++;
        var my_occuredBursts = occuredBursts.Pop();
        count = formations[my_occuredBursts >= formations.Length ? formations.Length - 1 : my_occuredBursts].formation_Base.GetFormation().CalculateNesting(ref occuredBursts, count);
        occuredBursts.Push(my_occuredBursts);
        return count;
    }

    public override bool IsComplete(ref Stack<int> occurredBursts)
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
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts >= formations.Length)
        {
            my_occuredBursts++;
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        position += positionOffset + formations[my_occuredBursts].positionOffset;


        formations[my_occuredBursts].formation_Base.GetFormation().UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling, position, 
            ref ex_elapsedTime, formations[my_occuredBursts].reversed ? !reversed : reversed);

        if (formations[my_occuredBursts].formation_Base.GetFormation().IsComplete(ref occurredBursts))
        {

            if (ex_elapsedTime.Count == 0)
            {
                Debug.LogWarning("Attempted to pop from an empty stack in MultiFormation Update.");
                return occurredBursts;
            }

            float my_ElaspedTime = ex_elapsedTime.Pop();

            if (formations[my_occuredBursts].formation_Base.GetFormation().IncrementElapsedTime())
                my_ElaspedTime = elapsedTime;

            // Shouldn't remove items on final index- burst increments after this hence -2
            if (my_occuredBursts <= formations.Length - 2)
            {
                Depth i = formations[my_occuredBursts].formation_Base.GetFormation().CalculateNesting(ref occurredBursts, new Depth() { nestDepth = 0, layerDepth = 0 });

                for (int j = i.nestDepth; j > 0; j--)
                {
                    occurredBursts.Pop();
                }

                for (int j = i.layerDepth; j > 0; j--)
                {
                    ex_elapsedTime.Pop();
                }

                // Pushing before setup as multiple ex_elaspedtime's only exist in layers=> we can only expect there to be a single exelapsedtime
                ex_elapsedTime.Push(my_ElaspedTime);
                my_occuredBursts++;
                formations[my_occuredBursts].formation_Base.GetFormation().SetUp(ref occurredBursts, ref ex_elapsedTime);
            }
            else
            {
                ex_elapsedTime.Push(my_ElaspedTime);
                my_occuredBursts++;
            }

        }

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
