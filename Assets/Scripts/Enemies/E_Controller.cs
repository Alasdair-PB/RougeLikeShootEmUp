using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class E_Controller : MonoBehaviour
{
    private float tolerance = .05f, moveVelocity = 3, timeAtLastAction;
    private float2 targetPos = new float2();
    private float2 nextPos = new float2();

    private StackManager<int> actionIndexStack = new StackManager<int>();
    private StackManager<float> actionTimeStampStack = new StackManager<float>();

    private int[] burstCounter = new int[5];  // Layer max for formations

    public void SetBurstCounter(int[] newBurstCounter) => burstCounter = newBurstCounter;
    public int[] GetBurstCounter() => burstCounter;
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

    public void PushActionTime(float timeStamp) => actionTimeStampStack.Push(timeStamp);
    public void ClearActionTimeStack() => actionTimeStampStack.Clear();
    public float GetActionTimeDirty() => actionTimeStampStack.Pop();
    public void ReturnLastItemToStackTimeModified(float newIndex) => actionTimeStampStack.ReturnLastItemToStackModified(newIndex);
    public void ReturnLastItemToStackTime() => actionTimeStampStack.ReturnLastItemToStack();

    public void PushActionIndex(int index) => actionIndexStack.Push(index);
    public void ClearActionStack() => actionIndexStack.Clear();
    public int GetActionIndexDirty() => actionIndexStack.Pop();
    public void ReturnLastItemToStackIndexModified(int newIndex) => actionIndexStack.ReturnLastItemToStackModified(newIndex);
    public void ReturnLastItemToStackIndex() => actionIndexStack.ReturnLastItemToStack();
}

public class StackManager<T>
{
    private Stack<T> mainStack = new Stack<T>();
    private Stack<T> tempStack = new Stack<T>();

    public void Push(T item) => mainStack.Push(item);
    public T Pop()
    {
        if (mainStack.Count > 0)
        {
            T item = mainStack.Pop();
            tempStack.Push(item);
            return item;
        }
        throw new System.Exception("Stack has been depleted");
    }

    public void Clear()
    {
        tempStack.Clear();
        mainStack.Clear();
    }

    public void ReturnLastItemToStackModified(T newItem)
    {
        if (tempStack.Count > 0)
        {
            tempStack.Pop();
            mainStack.Push(newItem);
        }
    }

    public void ReturnLastItemToStack()
    {
        if (tempStack.Count > 0)
        {
            T item = tempStack.Pop();
            mainStack.Push(item);
        }
    }
}

