using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Layered_Formation")]
public class Layered_Formation : Formation_Base
{
    public Formation_Base[] formations;

    public override Stack<int> SetUp(ref Stack<int> occuredBursts, ref Stack<float> ex_elapsedTime)
    {
        base.SetUp(ref occuredBursts, ref ex_elapsedTime);

        for (int i = 0; i < formations.Length; i++)
        {
            ex_elapsedTime.Push(ex_elapsedTime.Peek());
            occuredBursts = formations[i].SetUp(ref occuredBursts, ref ex_elapsedTime);
        }

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

            for (int j = nestingCount.nestDepth; j > 0; j--)
            {
                nestStack.Push(occuredBursts.Pop());
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
        var my_occuredBursts = occurredBursts.Pop();

        for (int i = 0; i < formations.Length; i++)
        {
            bool isCompleted = (my_occuredBursts & (1 << i)) != 0;

            if (!isCompleted)
            {
                occurredBursts.Push(my_occuredBursts);
                return false;
            }
            else
                continue;
        }
        occurredBursts.Push(my_occuredBursts);
        return true;
    }


    public override bool IncrementElapsedTime() => false;

    public override Stack<int> UpdateFormation(LayerMask layerMask,ref Stack<int> occurredBursts, float elapsedTime,
        GlobalPooling pooling, float2 position, ref Stack<float> ex_elapsedTime, bool reversed)
    {
        position += positionOffset;
        var my_occuredBursts = occurredBursts.Pop();

        Depth nestingCount = new Depth() { nestDepth = 0, layerDepth = 0 };
        Stack<int> depthStack = new Stack<int>();
        Stack<float> layerStack = new Stack<float>();

        layerStack.Push(ex_elapsedTime.Pop());

        //PrintStack(occurredBursts);

        for (int i = 0; i < formations.Length; i++)
        {
            for (int j = 0; j < nestingCount.nestDepth; j++)
            {
                if (j < nestingCount.layerDepth - 1)
                    layerStack.Push(ex_elapsedTime.Pop());
                depthStack.Push(occurredBursts.Pop());
            }

            bool isCompleted = (my_occuredBursts & (1 << i)) != 0;

            if (!isCompleted && !formations[i].IsComplete(ref occurredBursts))
            {
                formations[i].UpdateFormation(layerMask, ref occurredBursts, elapsedTime, pooling, position, ref layerStack, reversed);
                nestingCount = formations[i].CalculateNesting(ref occurredBursts, new Depth() { nestDepth = 0, layerDepth = 0 });
            }
            else if (!isCompleted)
            {  // First time completed
                my_occuredBursts |= (1 << i);
                Debug.Log("Completed!!");
                nestingCount.nestDepth = 0;
                nestingCount.layerDepth = 1;
            }
        }

        ReturnElementsToStack(ref occurredBursts, ref ex_elapsedTime, depthStack, layerStack);

        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }

    private void ReturnElementsToStack(ref Stack<int> occurredBursts, ref Stack<float> ex_elapsedTime, Stack<int> depthStack, Stack<float> layerStack) {

        foreach (int element in depthStack)
        {
            occurredBursts.Push(element);
        }

        foreach (int element in layerStack)
        {
            ex_elapsedTime.Push(element);
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


