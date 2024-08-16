using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class EnemyScheduler : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] EnemySchedule[] schedule;
        [SerializeField] Game game; 
        [SerializeField] float2 xBounds, yBounds;

        private int index, enemyCount, enemiesDestroyed;
        private float startTime, pauseTime;

        private Dictionary<GameObject, EnemyPooling> globalObjectPool = new Dictionary<GameObject, EnemyPooling>();

        public EnemyPooling GetObjectPool(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent = null)
        {
            if (!globalObjectPool.ContainsKey(prefab))
            {
                EnemyPooling newPool = new EnemyPooling(prefab, initialCapacity, maxCapacity, parent);
                globalObjectPool[prefab] = newPool;
            }
            return globalObjectPool[prefab];
        }

        private void OnEnable()
        {
            game.Reset += ClearAllPools;
            game.StartGame += OnGameStart;
        }

        private void OnDisable()
        {
            game.Reset -= ClearAllPools;
            game.StartGame -= OnGameStart;
        }

        private void ClearAllPools()
        {
            foreach (var item in globalObjectPool)
            {
                item.Value.ClearPool();
            }
        }

        private void Awake()
        {
            schedule = organizeSchedule(schedule);
            enemyCount = GetEnemyCount();
        }

        private void OnGameStart()
        {
            enemiesDestroyed = 0;
            index = 0;
            pauseTime = 0;
            startTime = Time.time;
        }

        private void Start()
        {
            OnGameStart();
        }

        private int GetEnemyCount()
        {
            int count = 0;
            for (int i = 0; i < schedule.Length; i++)
            {
                if (schedule[i].active)
                    count++;
            }
            return count;
        }

        private void FixedUpdate()
        {
            var time = Time.time - startTime;

            if (index > schedule.Length - 1)
                return;

            if (!schedule[index].active)
            {
                index++;
                return;
            }

            if (schedule[index].timeStamp < time)
            {
                if (enemiesDestroyed >= schedule[index].spawnOnXEnemiesDefeated)
                {
                    if (pauseTime > 0)
                        startTime = (Time.time - pauseTime) + startTime;

                    SpawnEnemy();
                    pauseTime = 0;
                    index++;
                } else if (!(pauseTime > 0))
                    pauseTime = Time.time;
                
            }
        }

        public void CalculateEnemiesRemaining(E_Controller e_Controller)
        {
            enemiesDestroyed++;
            if (enemiesDestroyed >= enemyCount)
                game.EndGame?.Invoke(true);
        }

        private EnemySchedule[] organizeSchedule(EnemySchedule[] enemySchedule)
        {
            Array.Sort(enemySchedule, (a, b) => a.timeStamp.CompareTo(b.timeStamp));
            return enemySchedule;
        }
        private void SpawnEnemy()
        {
            var scheduledEnemy = schedule[index];
            var objectInPool = GetObjectPool(prefabs[scheduledEnemy.enemyIndex], 1, 999, this.transform);
            objectInPool.InstantiateEnemy(scheduledEnemy.direction, scheduledEnemy.spawnPosition, xBounds, yBounds, this);
        }

        [Serializable]
        public struct EnemySchedule
        {
            public bool active, flipOnX, flipOnY;
            public int enemyIndex;
            public float timeStamp;
            public int spawnOnXEnemiesDefeated; 
            public float2 direction;
            public float2 spawnPosition;
        }

    }
}
