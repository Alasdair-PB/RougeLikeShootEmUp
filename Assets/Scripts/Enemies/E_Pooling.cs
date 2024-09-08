using System;
using Unity.Mathematics;
using UnityEngine;
namespace Enemies
{

    public class EnemyPooling : IObjectPools
    {
        private GameObject enemyPrefab;
        private int initialPoolSize = 10;
        private int maxPoolSize = 999;
        private float2 xBounds, yBounds;

        private ObjectPool<E_Controller> enemyPool; // May want statemachine instead
        public void ClearPool() => enemyPool.ReturnAllToPool();
        public void MoveAllAlongPath(float elaspedTime, PatternBase pattern, float speed, float2 direction) 
            => enemyPool.MoveAllAlongPath(elaspedTime, pattern, speed, direction);

        public EnemyPooling(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent, float2 xBounds, float2 yBounds)
        {
            enemyPrefab = prefab;
            initialPoolSize = initialCapacity;
            maxPoolSize = maxCapacity;
            SetUp(parent, xBounds, yBounds);
        }

        // SetUp Pool system for this specific object
        private void SetUp(Transform transform, float2 xBounds, float2 yBounds)
        {
            enemyPool = new ObjectPool<E_Controller>(enemyPrefab.GetComponent<E_Controller>(), initialPoolSize, maxPoolSize, transform);
            this.xBounds = xBounds;
            this.yBounds = yBounds;
        }

        public void InstantiateObject(float2 direction, float2 position, bool flippedX, bool flippedY, EnemyScheduler enemyScheduler)
        {
            E_Controller enemy = enemyPool.Get(new float3 (position.x, position.y, 0));
            SetUpEnemy(enemy, direction, position, xBounds, yBounds, flippedX, flippedY, enemyScheduler);
        }

        private void SetUpEnemy(E_Controller enemy, float2 direction, float2 position, float2 xBounds, float2 yBounds, 
            bool flippedX, bool flippedY, EnemyScheduler enemyScheduler)
        {
            var eActions = enemy.transform.GetComponent<E_Actions>();
            enemy.SetBounds(xBounds, yBounds);
            enemy.SetFlipped(flippedX, flippedY);
            eActions.OnDeath = null;
            eActions.OnDeath += ReturnEnemy;
            eActions.OnDeath += enemyScheduler.CalculateEnemiesRemaining;
        }

        public void ReturnEnemy(E_Controller enemy)
        {
            enemyPool.ReturnToPool(enemy);
        }
    }
}