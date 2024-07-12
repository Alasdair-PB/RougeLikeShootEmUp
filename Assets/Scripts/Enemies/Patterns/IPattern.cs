using Unity.Mathematics;
using UnityEngine;
public interface IPattern
{
    public float2 MoveTowards(float2 currentPos, float2 target, float speed, float elapsedTime);
    public float2 LerpToPosition(float2 currentPos, float2 target, float time, float elapsedTime);
    public float2 MoveInDirection(float2 currentPos, float2 direction, float speed, float elapsedTime);


}
public abstract class PatternBase : ScriptableObject, IPattern
{
    public abstract float2 MoveTowards(float2 currentPos, float2 target, float speed, float elapsedTime);
    public abstract float2 LerpToPosition(float2 currentPos, float2 target, float time, float elapsedTime);
    public abstract float2 MoveInDirection(float2 currentPos, float2 direction, float speed, float elapsedTime);
}

