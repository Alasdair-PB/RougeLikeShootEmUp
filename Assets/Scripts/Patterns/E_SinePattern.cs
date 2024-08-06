using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SinePattern", menuName = "Patterns/SinePattern")]
public class E_SinePattern : PatternBase
{
    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    public override float2 MoveTowards(float2 currentPos, float2 target, float speed, float elapsedTime)
    {
        float sineOffset = amplitude * math.sin(frequency * elapsedTime);
        float2 direction = math.normalize(target - currentPos);
        float2 nextPos = currentPos + direction * speed * Time.deltaTime;
        nextPos.y += sineOffset;
        return nextPos;
    }

    public override float2 LerpToPosition(float2 currentPos, float2 target, float time, float elapsedTime)
    {
        float t = math.clamp(elapsedTime / time, 0.0f, 1.0f);
        float sineOffset = amplitude * math.sin(frequency * elapsedTime);
        float2 lerpedPos = math.lerp(currentPos, target, t);
        lerpedPos.y += sineOffset;
        return lerpedPos;
    }

    public override float2 MoveInDirection(float2 currentPos, float2 direction, float speed, float elapsedTime)
    {
        float2 target = currentPos + direction * speed * elapsedTime;
        return MoveTowards(currentPos, target, speed, elapsedTime);
    }

}
