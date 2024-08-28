using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Formation")]
public class Formation : Formation_Base
{
    public GameObject projectileObject;
    public float startDelay, burstTime;
    public int burstCount, angleChange;
    public Variation[] spawnInit;

    [Serializable]
    public struct Variation
    {
        public int angle;
        public float2 positionOffset;
    }


    // Actual stack: not a copy
    public override bool IsComplete(ref Stack<int> occurredBursts)
    {
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts >= burstCount)
        {
            occurredBursts.Push(my_occuredBursts);
            return true;
        }

        occurredBursts.Push(my_occuredBursts);
        return false;
    }

    public override bool IncrementElapsedTime() => true;

    public override Stack<int> UpdateFormation(LayerMask layerMask, ref Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, 
        float2 position, ref Stack<float> ex_elapsedTime, bool reversed)
    {
        position += positionOffset;
        var my_occuredBursts = occurredBursts.Pop();
        float my_ElapsedTime = elapsedTime - ex_elapsedTime.Peek();

        int burstsTriggered = Mathf.FloorToInt(my_ElapsedTime / burstTime);

        if (my_occuredBursts > burstsTriggered || my_ElapsedTime < startDelay)
        {
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        var angleOffset = angleChange * burstsTriggered;

        for (int i = reversed ? spawnInit.Length - 1 : 0; reversed? i >= 0 : i < spawnInit.Length; i += reversed? -1 : 1)
        {
            var objectInPool = pooling.GetProjectilePool(projectileObject, 10, 999);
            float degrees = reversed ? spawnInit[i].angle - angleOffset: spawnInit[i].angle + angleOffset;

            float radians = degrees * Mathf.Deg2Rad;
            float2 direction = new float2(Mathf.Cos(radians), Mathf.Sin(radians));

            objectInPool.InstantiateProjectile(direction, layerMask, position + spawnInit[i].positionOffset);
        }

        my_occuredBursts++;
        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
