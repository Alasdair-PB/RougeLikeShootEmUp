using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Formation")]

public class ScriptableFormation : Scriptable_FormationBase
{
    public Formation formation;
    public override Formation_Base GetFormation() => formation;
}



[Serializable]
public class Formation : Formation_Base
{
    public GameObject[] projectileObject;
    public float startDelay;
    public float[] burstTime, angleChange;
    public int burstCount;
    public Variation[] spawnInit;

    [Serializable]
    public struct Variation
    {
        public int angle, projectileIndex;
        public float2 positionOffset;
    }

    public override bool IsComplete(ref Stack<int> occurredBursts)
    {
        var my_occuredBursts = occurredBursts.Pop();

        if (my_occuredBursts > burstCount)
        {
            occurredBursts.Push(my_occuredBursts);
            return true;
        }

        occurredBursts.Push(my_occuredBursts);
        return false;
    }

    public override bool IncrementElapsedTime() => true;


    // Could be manually set in the scriptable object to save performance
    private int GetBurstTriggered(float my_ElapsedTime)
    {
        int burstsTriggered = 0;
        float remainingTime = my_ElapsedTime;

        float fullCycleTime = 0;
        for (int i = 0; i < burstTime.Length; i++)
        {
            fullCycleTime += burstTime[i];
        }

        int fullCycles = Mathf.FloorToInt(remainingTime / fullCycleTime);
        burstsTriggered += fullCycles * burstTime.Length;
        remainingTime -= fullCycles * fullCycleTime;

        for (int i = 0; i < burstTime.Length && remainingTime >= burstTime[i]; i++)
        {
            remainingTime -= burstTime[i];
            burstsTriggered++;
        }

        return burstsTriggered;
    }

    private float GetAngleOffset(int burstsTriggered)
    {
        float angleOffset = 0;

        float fullCycleAngleChange = 0;
        for (int i = 0; i < angleChange.Length; i++)
        {
            fullCycleAngleChange += angleChange[i];
        }

        int fullCycles = burstsTriggered / angleChange.Length;
        angleOffset += fullCycles * fullCycleAngleChange;

        for (int i = 0; i < burstsTriggered % angleChange.Length; i++)
        {
            angleOffset += angleChange[i];
        }
        return angleOffset;
    }

    public override Stack<int> UpdateFormation(LayerMask layerMask, ref Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling,
        float2 position, ref Stack<float> ex_elapsedTime, bool reversed)
    {
        position += positionOffset;
        var my_occuredBursts = occurredBursts.Pop();

        float my_ElapsedTime = elapsedTime - ex_elapsedTime.Peek();
        var burstsTriggered = GetBurstTriggered(my_ElapsedTime);

        if (my_occuredBursts > burstsTriggered || my_ElapsedTime < startDelay)
        {

            if (my_occuredBursts >= burstCount)
                my_occuredBursts++;

            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        float angleOffset = GetAngleOffset(burstsTriggered);

        for (int i = reversed ? spawnInit.Length - 1 : 0; reversed ? i >= 0 : i < spawnInit.Length; i += reversed ? -1 : 1)
        {
            var objectInPool = pooling.GetProjectilePool(projectileObject[spawnInit[i].projectileIndex], 10, 999);
            float degrees = reversed ? spawnInit[i].angle - angleOffset : spawnInit[i].angle + angleOffset;

            float radians = degrees * Mathf.Deg2Rad;
            float2 direction = new float2(Mathf.Cos(radians), Mathf.Sin(radians));

            objectInPool.InstantiateProjectile(direction, layerMask, position + spawnInit[i].positionOffset);
        }

        my_occuredBursts++;
        occurredBursts.Push(my_occuredBursts);
        return occurredBursts;
    }
}
