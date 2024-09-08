using Enemies;
using Unity.Mathematics;


public interface IObjectPools 
{
    public void ClearPool();
    public void InstantiateObject(float2 direction, float2 position, bool flippedX, bool flippedY, EnemyScheduler enemyScheduler);
    public void MoveAllAlongPath(float elaspedTime, PatternBase pattern, float speed, float2 direction);
}

