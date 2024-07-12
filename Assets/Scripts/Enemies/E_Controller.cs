using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class E_Controller : MonoBehaviour
{
    private float tolerance = .05f, moveVelocity = 3, timeAtLastAction;
    private float2 targetPos = new float2();
    private float2 nextPos = new float2();

    private Stack<int> actionIndexStack = new Stack<int>(), tempStack = new Stack<int>();
    private Stack<float> actionTimeStampStack = new Stack<float>(), tempFloatStack = new Stack<float>(); 

    public float2 GetCurrentPosition()
        => new float2(transform.position.x, transform.position.y);
    public bool IsAtPosition(float2 currentPos, float2 expectedPos)
        => math.distance(currentPos, expectedPos) <= tolerance;
    public float2 GetTargetPosition() => targetPos;
    public float2 SetTargetPosition(float2 newTarget) => targetPos = newTarget;
    public void SetNextPosition(float2 newPos) => nextPos = newPos;

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(nextPos.x, nextPos.y, 0);
    }


    public void PushActionIndex(int index) => actionIndexStack.Push(index);
    public void PushActionTime(float timeStamp) => actionTimeStampStack.Push(timeStamp);
    public void ClearActionStack()
    {
        tempStack.Clear();
        actionIndexStack.Clear();
    }
    public void ClearActionTimeStack()
    {
        tempFloatStack.Clear();
        actionTimeStampStack.Clear();
    }

    public float GetActionTimeDirty()
    {
        if (actionTimeStampStack.Count > 0)
        {
            float index = actionTimeStampStack.Pop();
            tempFloatStack.Push(index);
            return index;
        }
        throw new System.Exception("Stack has been depleted");
    }

    public void ReturnLastItemToStackTimeModified(float newIndex)
    {
        if (tempFloatStack.Count > 0)
        {
            tempFloatStack.Pop();
            actionTimeStampStack.Push(newIndex);
        }
    }

    public void ReturnLastItemToStackTime()
    {
        if (tempFloatStack.Count > 0)
        {
            float index = tempFloatStack.Pop();
            actionTimeStampStack.Push(index);
        }
    }

    public int GetActionIndexDirty()
    {
        if (actionIndexStack.Count > 0)
        {
            int index = actionIndexStack.Pop();
            tempStack.Push(index);
            return index;
        }
        throw new System.Exception("Stack has been depleted");
    }

    public void ReturnLastItemToStackIndexModified(int newIndex)
    {
        if (tempStack.Count > 0)
        {
            tempStack.Pop();
            actionIndexStack.Push(newIndex);
        }
    }

    public void ReturnLastItemToStackIndex()
    {
        if (tempStack.Count > 0)
        {
            int index = tempStack.Pop();
            actionIndexStack.Push(index);
        }
    }
}
