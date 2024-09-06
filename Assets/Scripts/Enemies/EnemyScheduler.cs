using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class EnemyScheduler : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private GameObject[] backgroundPrefabs;

        [SerializeField] EnemySchedule[] schedule;
        [SerializeField] BackgroundSchedule[] backgroundSchedule;
        private int[] activeBackgroundCount; 

        [SerializeField] Game game; 
        [SerializeField] float2 xBounds, yBounds;

        private int index, backgroundIndex, enemyCount, enemiesDestroyed;
        private float startTime, pauseTime, bPauseTime;

        private Dictionary<GameObject, EnemyPooling> globalObjectPool = new Dictionary<GameObject, EnemyPooling>();
        private Dictionary<GameObject, Background_Pooling> globalBackgroundPool = new Dictionary<GameObject, Background_Pooling>();

        public EnemyPooling GetObjectPool(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent = null)
        {
            if (!globalObjectPool.ContainsKey(prefab))
            {
                EnemyPooling newPool = new EnemyPooling(prefab, initialCapacity, maxCapacity, parent);
                globalObjectPool[prefab] = newPool;
            }
            return globalObjectPool[prefab];
        }

        public Background_Pooling GetObjectBackgroundPool(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent = null)
        {
            if (!globalBackgroundPool.ContainsKey(prefab))
            {
                Background_Pooling newPool = new Background_Pooling(prefab, initialCapacity, maxCapacity, parent);
                globalBackgroundPool[prefab] = newPool;
            }
            return globalBackgroundPool[prefab];
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

            foreach (var item in globalBackgroundPool)
            {
                item.Value.ClearPool();
            }
        }

        private void Awake()
        {
            schedule = organizeSchedule(schedule);
            backgroundSchedule = organizeSchedule(backgroundSchedule);
            enemyCount = GetEnemyCount();
            activeBackgroundCount = new int[backgroundSchedule.Length]; 
        }

        private void OnGameStart()
        {
            enemiesDestroyed = 0;
            index = 0;
            pauseTime = 0;
            bPauseTime = 0;
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

            if (index > schedule.Length - 1 && backgroundIndex > backgroundSchedule.Length - 1)
                return;
            SpawnEnemyOnSchedule(time);
            SpawnBackgroundOnSchedule(time);
            UpdateActiveBackgroundElements();
        }

        private void SpawnEnemyOnSchedule(float time)
        {
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
                }
                else if (!(pauseTime > 0))
                    pauseTime = Time.time;
            }
        }

        private void SpawnBackgroundOnSchedule(float time)
        {
            if (!backgroundSchedule[backgroundIndex].active)
            {
                backgroundIndex++;
                return;
            }

            if (backgroundSchedule[backgroundIndex].timeStamp < time)
            {
                if (enemiesDestroyed >= backgroundSchedule[backgroundIndex].spawnOnXEnemiesDefeated)
                {
                    if (bPauseTime > 0)
                        startTime = (Time.time - bPauseTime) + startTime;

                    SpawnBackgroundItem();
                    bPauseTime = 0;
                    backgroundIndex++;
                }
                else if (!(bPauseTime > 0))
                    bPauseTime = Time.time;
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

        private BackgroundSchedule[] organizeSchedule(BackgroundSchedule[] backgroundSchedule)
        {
            Array.Sort(backgroundSchedule, (a, b) => a.timeStamp.CompareTo(b.timeStamp));
            return backgroundSchedule;
        }

        private void SpawnBackgroundItem()
        {
            var scheduledBackgroundItem = backgroundSchedule[backgroundIndex];
            var objectInPool = GetObjectBackgroundPool(prefabs[scheduledBackgroundItem.backgroundIndex], 1, 999, this.transform);
            objectInPool.InstantiateBackground(scheduledBackgroundItem.direction, scheduledBackgroundItem.spawnPosition);
            // Add background item to tracked list
        }

        private void UpdateActiveBackgroundElements()
        {
            // Get Tracked list
            // Move background items back based on Pattern
            // Check if backgrond items leave x/y bounds
            // Respawn items 
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

        [Serializable]
        public struct BackgroundSchedule
        {
            public int waitOnEnemyScheduleIndex; //0 will require no waiting- refers to organized scheule rather than serialized one
            public int spawnOnXEnemiesDefeated, stopSpawnOnXEnemiesDefeated, backgroundIndex;
            public float timeStamp, respawnTime, stopAtTimeStamp;
            public int repeatCount; // Ignored if looped
            public bool active, loop, randomizePosition;

            public float2 direction, spawnPosition;

        }

    }
}
