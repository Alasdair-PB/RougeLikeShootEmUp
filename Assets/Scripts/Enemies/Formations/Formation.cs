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
    public override bool IsComplete(Stack<int> occurredBursts, float elapsedTime, float2 position)
    {
        var my_occuredBursts = occurredBursts.Pop();
        bool isComplete = false;

        if (my_occuredBursts >= burstCount)
            isComplete =  true;


        occurredBursts.Push(my_occuredBursts);

        Debug.Log(isComplete);

        return isComplete;
    }


    public override Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position)
    {        
        var my_occuredBursts = occurredBursts.Pop();
        int burstsTriggered = Mathf.FloorToInt(elapsedTime / burstTime);

        if (my_occuredBursts > burstsTriggered || elapsedTime < startDelay)
        {
            occurredBursts.Push(my_occuredBursts);
            return occurredBursts;
        }

        var angleOffset = angleChange * burstsTriggered;

        for (int i = 0; i < angle.Length; i++)
        {
            var objectInPool = pooling.GetProjectilePool(projectileObject, 10, 999);

            float degrees = angle[i] + angleOffset;
            float radians = degrees * Mathf.Deg2Rad;
            float2 direction = new float2(Mathf.Cos(radians), Mathf.Sin(radians));

            objectInPool.InstantiateProjectile(direction, layerMask, position);
        }

        my_occuredBursts++;
        occurredBursts.Push(my_occuredBursts);

        return occurredBursts;
    }
}
