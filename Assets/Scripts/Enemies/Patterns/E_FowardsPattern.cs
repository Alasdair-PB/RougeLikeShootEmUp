using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "ForwardsPattern", menuName = "Patterns/ForwardsPattern")]
public class E_Fowards : PatternBase
{

    public override float2 MoveTowards(float2 currentPos, float2 target, float speed, float elapsedTime)
    {
        float2 direction = math.normalize(target - currentPos);
        float2 nextPos = currentPos + direction * speed * Time.deltaTime;
        return nextPos;
    }

    public override float2 LerpToPosition(float2 currentPos, float2 target, float time, float elapsedTime)
    {
        float t = math.clamp(elapsedTime / time, 0.0f, 1.0f);
        float2 lerpedPos = new float2(Mathf.Lerp(currentPos.x, target.x, t), Mathf.Lerp(currentPos.y, target.y, t));
        return lerpedPos;
    }


    public override float2 MoveInDirection(float2 currentPos, float2 direction, float speed, float elapsedTime)
    {
        float2 target = currentPos + direction * speed * elapsedTime;
        return MoveTowards(currentPos, target, speed, elapsedTime);
    }

}
