using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Formation")]
public class Formation : ScriptableObject
{
    public GameObject projectileObject;
    public float startDelay, burstTime;
    public int burstCount, angleChange;
    public float[] angle;


    public int UpdateFormation(LayerMask layerMask, int occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position)
    {

        if (elapsedTime < startDelay)
            return occurredBursts;

        if (occurredBursts >= burstCount)
            return occurredBursts;

        int burstsTriggered = Mathf.FloorToInt(elapsedTime / burstTime);
        if (occurredBursts > burstsTriggered)
            return occurredBursts;

        //float nextBurstTime = burstsTriggered * burstTime + startDelay;

        var angleOffset = angleChange * burstsTriggered;

        for (int i = 0; i < angle.Length; i++)
        {
            var objectInPool = pooling.GetPool(projectileObject, 10, 999);

            float degrees = angle[i] + angleOffset;
            float radians = degrees * Mathf.Deg2Rad;
            float2 direction = new float2(Mathf.Cos(radians), Mathf.Sin(radians));

            objectInPool.InstantiateProjectile(direction, layerMask,
                new float2(position.x, position.y));
        }

        occurredBursts++;
        return occurredBursts;
    }



}
