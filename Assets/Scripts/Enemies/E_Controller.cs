using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{

    [RequireComponent(typeof(E_Actions))]
    public class E_Controller : MonoBehaviour
    {
        private E_Actions e_Actions;
        private float tolerance = .05f, moveVelocity = 3, timeAtLastAction;
        private float2 targetPos = new float2(), nextPos = new float2(), startPos, xBounds = new float2(), yBounds = new float2();

        private StackManager<int> actionIndexStack = new StackManager<int>();
        private StackManager<float> actionTimeStampStack = new StackManager<float>();
        private Stack<float> ex_ElapsedTime = new Stack<float>(new float[] { 0.0f });
        private bool flipOnX, flipOnY;

        public bool GetFlippedX() => flipOnX;
        public bool GetFlippedY() => flipOnY;

        private Stack<int>[] burstCounter = new Stack<int>[5];  // Layer max for formations

        public void SetStartPos(float2 newStartPos) => startPos = newStartPos;
        private void Awake() => e_Actions = GetComponent<E_Actions>();

        public void OnEnable()
        {
            startPos = new float2(transform.position.x, transform.position.y);
            SetNextPosition(startPos);
            UpdatePosition();
        }

        public Stack<float> GetEx_ElapsedTime() => ex_ElapsedTime;
        public void SetEx_ElapsedTime(Stack<float> newElapse) => ex_ElapsedTime = newElapse;

        public void SetBounds(float2 xBounds, float2 yBounds)
        {
            this.xBounds = xBounds;
            this.yBounds = yBounds;
        }

        public void SetFlipped(bool flippedOnX, bool flippedOnY)
        {
            flipOnX = flippedOnX;
            flipOnY = flippedOnY;
        }


        void FixedUpdate()
        {
            if (nextPos.x < xBounds.x || nextPos.x > xBounds.y || nextPos.y < yBounds.x || nextPos.y > yBounds.y)
            {
                e_Actions.OnDeath?.Invoke(this);
                return;
            }

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position = new Vector3(nextPos.x, nextPos.y, 0);
        }

        //-------------------------------------Position edits-----------------------------------------
        public float2 GetCurrentPosition()
        => new float2(transform.position.x, transform.position.y);
        public bool IsAtPosition(float2 currentPos, float2 expectedPos)
            => math.distance(currentPos, expectedPos) <= tolerance;
        public float2 GetTargetPosition() => targetPos;
        public float2 SetTargetPosition(float2 newTarget) => targetPos = newTarget;
        public void SetNextPosition(float2 newPos) => nextPos = newPos;



        //-------------------------------------Burst Counter calls-----------------------------------------

        public void ClearBurstCounter() => burstCounter = new Stack<int>[5];
        public void SetBurstCounter(Stack<int>[] newBurstCounter) => burstCounter = newBurstCounter;
        public Stack<int>[] GetBurstCounter() => burstCounter;

        //-------------------------------------Elapsed time stack calls-----------------------------------------

        public void PushActionTime(float timeStamp) => actionTimeStampStack.Push(timeStamp);
        public void ClearActionTimeStack() => actionTimeStampStack.Clear();
        public float GetActionTimeDirty() => actionTimeStampStack.Pop();
        public void ReturnLastItemToStackTimeModified(float newIndex) => actionTimeStampStack.ReturnLastItemToStackModified(newIndex);
        public void ReturnLastItemToStackTime() => actionTimeStampStack.ReturnLastItemToStack();

        //-------------------------------------Current action index stack calls-----------------------------------------

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
}

