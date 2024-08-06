using Unity.Mathematics;
using UnityEngine;

public class EnemyPooling
{
    private GameObject enemyPrefab;
    private int initialPoolSize = 10;
    private int maxPoolSize = 999;

    private ObjectPool<E_Controller> enemyPool; // May want statemachine instead

    public EnemyPooling(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent)
    {
        enemyPrefab = prefab;
        initialPoolSize = initialCapacity;
        maxPoolSize = maxCapacity;
        SetUp(parent);
    }

    // SetUp Pool system for this specific object
    private void SetUp(Transform transform)
    {
        enemyPool = new ObjectPool<E_Controller>(enemyPrefab.GetComponent<E_Controller>(), initialPoolSize, maxPoolSize, transform);
    }

    public void InstantiateEnemy(float2 direction, float2 position, float2 xBounds, float2 yBounds)
    {
        E_Controller enemy = enemyPool.Get();
        SetUpEnemy(enemy, direction, position, xBounds, yBounds);
    }

    private void SetUpEnemy(E_Controller enemy, float2 direction, float2 position, float2 xBounds, float2 yBounds)
    {
        enemy.transform.position = new Vector3(position.x, position.y, 0);
        enemy.SetBounds(xBounds, yBounds);
        enemy.OnDeath = null;
        enemy.OnDeath += ReturnEnemy;
        enemy.InitializeEnemy(direction);
    }

    public void ReturnEnemy(E_Controller enemy)
    {
        enemyPool.ReturnToPool(enemy);
    }

    // Finish this


}