using System;
using Unity.Mathematics;
using UnityEngine;
namespace Enemies
{

    public class EnemyPooling
    {
        private GameObject enemyPrefab;
        private int initialPoolSize = 10;
        private int maxPoolSize = 999;

        private ObjectPool<E_Controller> enemyPool; // May want statemachine instead

        public void ClearPool() => enemyPool.ReturnAllToPool();

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

        public void InstantiateEnemy(float2 direction, float2 position, float2 xBounds, float2 yBounds, EnemyScheduler enemyScheduler)
        {
            E_Controller enemy = enemyPool.Get();
            SetUpEnemy(enemy, direction, position, xBounds, yBounds, enemyScheduler);
        }

        private void SetUpEnemy(E_Controller enemy, float2 direction, float2 position, float2 xBounds, float2 yBounds, EnemyScheduler enemyScheduler)
        {

            var eActions = enemy.transform.GetComponent<E_Actions>();
            enemy.transform.position = new Vector3(position.x, position.y, 0);
            enemy.SetBounds(xBounds, yBounds);
            eActions.OnDeath = null;
            eActions.OnDeath += ReturnEnemy;
            eActions.OnDeath += enemyScheduler.CalculateEnemiesRemaining;
            enemy.InitializeEnemy(direction, position);
        }


        public void ReturnEnemy(E_Controller enemy)
        {
            enemyPool.ReturnToPool(enemy);
        }

        // Finish this


    }
}