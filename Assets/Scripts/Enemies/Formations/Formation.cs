using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Formation")]
public class Formation : Formation_Base
{
    public GameObject projectileObject;
    public float startDelay, burstTime;
    public int burstCount, angleChange;
    public float[] angle;


    // Actual stack: not a copy
    public override bool IsComplete(ref Stack<int> occurredBursts, float elapsedTime, float ex_elapsedTime, float2 position)
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

    public override Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, 
        float2 position, ref float ex_elapsedTime, bool reversed)
    {        
        var my_occuredBursts = occurredBursts.Pop();
        var my_ElapsedTime = elapsedTime - ex_elapsedTime;

        int burstsTriggered = Mathf.FloorToInt(my_ElapsedTime / burstTime);

        if (my_occuredBursts > burstsTriggered || my_ElapsedTime < startDelay)
        {
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        var angleOffset = angleChange * burstsTriggered;

        for (int i = reversed ? angle.Length - 1 : 0; reversed? i >= 0 : i < angle.Length; i += reversed? -1 : 1)
        {
            var objectInPool = pooling.GetProjectilePool(projectileObject, 10, 999);
            float degrees = reversed ? angle[i] - angleOffset: angle[i] + angleOffset;

            float radians = degrees * Mathf.Deg2Rad;
            float2 direction = new float2(Mathf.Cos(radians), Mathf.Sin(radians));

            objectInPool.InstantiateProjectile(direction, layerMask, position);
        }

        my_occuredBursts++;

        occurredBursts.Push(my_occuredBursts);

        return occurredBursts;
    }
}
