using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Layered_Formation")]
public class Layered_Formation : Formation_Base
{
    public Formation_Base[] formations;
    public override bool IncrementElapsedTime() => false;

    public override Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime)
    {
        base.SetUp(ref occuredBursts, ref ex_elapsedTime);
        var my_ElaspedTime = ex_elapsedTime.Peek();

        for (int i = 0; i < formations.Length; i++)
        {
            occuredBursts = formations[i].SetUp(ref occuredBursts, ref ex_elapsedTime);

            if (i != 0)
                ex_elapsedTime.Push(my_ElaspedTime);
        }

        ex_elapsedTime.Push(my_ElaspedTime);
        return occuredBursts;
    }

    public override Depth CalculateNesting(ref Stack<int> occuredBursts, Depth count)
    {
        count.nestDepth++;
        var my_occuredBursts = occuredBursts.Pop();

        Depth nestingCount = new Depth() { layerDepth = 0, nestDepth = 0 };
        Stack<int> nestStack = new Stack<int>();

        for (int i = 0; i < formations.Length; i++)
        {
            count.layerDepth++;

            while (nestingCount.nestDepth > 0)
            {
                if (occuredBursts.Count > 0)
                {
                    nestStack.Push(occuredBursts.Pop());
                    nestingCount.nestDepth--;
                }
                else
                {
                    Debug.LogWarning("Attempted to pop from an empty stack in CalculateNesting.");
                    break;
                }
            }

            nestingCount = formations[i].CalculateNesting(ref occuredBursts, new Depth() { layerDepth = 0, nestDepth = 0 });
            count.nestDepth += nestingCount.nestDepth;
            count.layerDepth += nestingCount.layerDepth;
        }

        foreach (int element in nestStack)
        {
            occuredBursts.Push(element);
        }
        occuredBursts.Push(my_occuredBursts);
        return count;
    }

    public override bool IsComplete(ref Stack<int> occurredBursts)
    {
        if (occurredBursts.Count == 0)
        {
            Debug.LogWarning("Attempted to pop from an empty stack in IsComplete.");
            return false;
        }

        var my_occuredBursts = occurredBursts.Pop();

        for (int i = 0; i < formations.Length; i++)
        {
            bool isCompleted = (my_occuredBursts & (1 << i)) != 0;

            if (!isCompleted)
            {
                occurredBursts.Push(my_occuredBursts);
                return false;
            }
        }
        occurredBursts.Push(my_occuredBursts);
        return true;
    }

    public override Stack<int> UpdateFormation(LayerMask layerMask, ref Stack<int> occurredBursts, float elapsedTime,
        GlobalPooling pooling, float2 position, ref Stack<float> ex_elapsedTime, bool reversed)
    {
        position += positionOffset;

        if (occurredBursts.Count == 0)
        {
            Debug.LogWarning("Attempted to pop from an empty stack in UpdateFormation.");
            return occurredBursts;
        }

        var my_occuredBursts = occurredBursts.Pop();

        Depth nestingCount = new Depth() { nestDepth = 0, layerDepth = 0 };
        Stack<int> depthStack = new Stack<int>();
        Stack<float> layerStack = new Stack<float>();

        for (int i = 0; i < formations.Length; i++)
        {

            for (int j = 0; j < nestingCount.nestDepth; j++)
            {
                if (occurredBursts.Count > 0)
                    depthStack.Push(occurredBursts.Pop());
                else
                {
                    Debug.LogWarning("Attempted to pop from an empty stack in UpdateFormation.");
                    break;
                }
            }

            for (int j = 0; j < nestingCount.layerDepth; j++)
            {
                if (ex_elapsedTime.Count > 0)
                {
                    layerStack.Push(ex_elapsedTime.Pop());
                }
                else
                {
                    Debug.LogWarning("Attempted to pop from an empty stack in UpdateFormation.");
                    break;
                }
            }

            bool isCompleted = (my_occuredBursts & (1 << i)) != 0;

            if (!isCompleted && !formations[i].IsComplete(ref occurredBursts))
            {
                formations[i].UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling, position, ref ex_elapsedTime, reversed);
            }
            else if (!isCompleted)
            {
                my_occuredBursts |= (1 << i);
                // Breaking to reset nest count- delays other shots by single frame
                break;
            }
            nestingCount = formations[i].CalculateNesting(ref occurredBursts, new Depth() { nestDepth = 0, layerDepth = 0 });
        }

        ReturnElementsToStack(ref occurredBursts, ref ex_elapsedTime, depthStack, layerStack);
        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }

    private void ReturnElementsToStack(ref Stack<int> occurredBursts, ref Stack<float> ex_elapsedTime, Stack<int> depthStack, Stack<float> layerStack)
    {
        while (depthStack.Count > 0)
        {
            occurredBursts.Push(depthStack.Pop());
        }

        while (layerStack.Count > 0)
        {
            ex_elapsedTime.Push(layerStack.Pop());
        }
    }

    private void PrintStack(Stack<float> occurredBursts)
    {
        string outString = "";
        foreach (var element in occurredBursts)
        {
            outString += element.ToString() + ", ";
        }
        Debug.Log(outString);
    }

    private void PrintStack(Stack<int> occurredBursts)
    {
        string outString = "";
        foreach (var element in occurredBursts)
        {
            outString += element.ToString() + ", ";
        }
        Debug.Log(outString);
    }
}
