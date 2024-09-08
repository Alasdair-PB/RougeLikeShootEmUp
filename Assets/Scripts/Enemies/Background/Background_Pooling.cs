using Enemies;
using Unity.Mathematics;
using UnityEngine;

public class Background_Pooling : IObjectPools
{
    private GameObject backgroundPrefab;
    private int initialPoolSize = 10;
    private int maxPoolSize = 999;
    private float2 yBounds = new float2(), xBounds = new float2();

    private ObjectPool<BackgroundItem> backgroundPool;

    public void ClearPool() => backgroundPool.ReturnAllToPool();

    public void MoveAllAlongPath(float elaspedTime, PatternBase pattern, float speed, float2 direction) 
        => backgroundPool.MoveAllAlongPath(elaspedTime, pattern, speed, direction);

    public Background_Pooling(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent, float2 newXBounds, float2 newYBounds)
    {
        backgroundPrefab = prefab;
        initialPoolSize = initialCapacity;
        maxPoolSize = maxCapacity;
        xBounds = newXBounds;
        yBounds = newYBounds;
        SetUp(parent);
    }

    private void SetUp(Transform transform)
    {
        backgroundPool = new ObjectPool<BackgroundItem>(backgroundPrefab.GetComponent<BackgroundItem>(), initialPoolSize, maxPoolSize, transform);
    }

    public void InstantiateObject(float2 direction, float2 position, bool flippedX, bool flippedY, EnemyScheduler enemyScheduler)
    {
        BackgroundItem backgroundItem = backgroundPool.Get();
        SetUpBackgroundItem(backgroundItem, direction, position);
    }

    private void SetUpBackgroundItem(BackgroundItem backgroundItem, float2 direction, float2 position)
    {
        backgroundItem.transform.position = new Vector3(position.x, position.y, 1.8f);
        backgroundItem.OnReturn = null;
        backgroundItem.SetBounds(xBounds, yBounds);
        backgroundItem.OnReturn += ReturnBackgroundItem;
    }

    public void ReturnBackgroundItem(BackgroundItem backgroundItem)
    {
        backgroundPool.ReturnToPool(backgroundItem);
    }
}