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
        [SerializeField] float2 xBounds, yBounds, xBoundBackground, yBoundsBackground;
        [SerializeField] PatternBase pattern;
        [SerializeField] float backgroundSpeed;
        [SerializeField] float2 backgroundDirection;

        private int index, backgroundIndex, enemyCount, enemiesDestroyed;
        private float startTime, pauseTime, bPauseTime, bStartTime, elaspedTime;

        private Dictionary<GameObject, IObjectPools> globalObjectPool = new Dictionary<GameObject, IObjectPools>();
        private Dictionary<GameObject, IObjectPools> globalPathedObjectPool = new Dictionary<GameObject, IObjectPools>();


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
            foreach (var item in globalPathedObjectPool)
            {
                item.Value.ClearPool();
            }
        }

        private void Awake()
        {
            organizeSchedule(schedule); 
            organizeSchedule(backgroundSchedule); 
            enemyCount = GetEnemyCount();
            activeBackgroundCount = new int[backgroundSchedule.Length]; 
        }

        private void OnGameStart()
        {
            enemiesDestroyed = 0;
            index = 0;
            backgroundIndex = 0;
            pauseTime = 0;
            bPauseTime = 0;
            startTime = Time.time;
            bStartTime = Time.time;
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
            if (index <= schedule.Length - 1)
                SpawnItemOnSchedule(Time.time - startTime, ref pauseTime, ref startTime, ref index, schedule, true);
            
            if (backgroundIndex <= backgroundSchedule.Length - 1)
                SpawnItemOnSchedule(Time.time - bStartTime, ref bPauseTime, ref bStartTime, ref backgroundIndex, backgroundSchedule, false);

            UpdateActiveElementsOnPath();
        }

        public IObjectPools GetObjectPool(Dictionary<GameObject, IObjectPools> myObjectPool,GameObject prefab, int initialCapacity, int maxCapacity, bool enemy, Transform parent = null)
        {
            if (!myObjectPool.ContainsKey(prefab))
            {
                IObjectPools newPool;

                if (enemy)
                    newPool = new EnemyPooling(prefab, initialCapacity, maxCapacity, parent, xBounds, yBounds);
                else
                    newPool = new Background_Pooling(prefab, initialCapacity, maxCapacity, parent, xBoundBackground, yBoundsBackground);

                myObjectPool[prefab] = newPool;
            }
            return myObjectPool[prefab];
        }

        private void SpawnItemOnSchedule(float time, ref float pauseTimer, ref float myStartTime, ref int indexVal, IScheduleItem[] mySchedule, bool enemy)
        {
            if (!mySchedule[indexVal].active)
            {
                indexVal++;
                return;
            }

            if (mySchedule[indexVal].timeStamp < time)
            {
                if (enemiesDestroyed >= mySchedule[indexVal].spawnOnXEnemiesDefeated)
                {
                    if (pauseTimer > 0)
                        myStartTime = (Time.time - pauseTimer) + myStartTime;

                    SpawnObject(indexVal, mySchedule, enemy);
                    pauseTimer = 0;
                    indexVal++;
                }
                else if (!(pauseTimer > 0))
                    pauseTimer = Time.time;
            }
        }

        public void CalculateEnemiesRemaining(E_Controller e_Controller)
        {
            enemiesDestroyed++;
            if (enemiesDestroyed >= enemyCount)
                game.EndGame?.Invoke(true);
        }

        private IScheduleItem[] organizeSchedule(IScheduleItem[] mySchedule)
        {
            Array.Sort(mySchedule, (a, b) => a.timeStamp.CompareTo(b.timeStamp));
            return mySchedule;
        }


        private void SpawnObject(int indexVal, IScheduleItem[] mySchedule, bool enemy)
        {
            var item = mySchedule[indexVal];
            IObjectPools objectInPool;

            if (enemy)
                objectInPool = GetObjectPool(globalObjectPool, prefabs[item.index], 1, 999, enemy, this.transform);
            else
                objectInPool = GetObjectPool(globalPathedObjectPool, backgroundPrefabs[item.index], 1, 999, enemy, this.transform);
            objectInPool.InstantiateObject(item.direction, item.spawnPosition, this);
        }

        private void UpdateActiveElementsOnPath()
        {
            elaspedTime += Time.deltaTime;
            foreach (var item in globalPathedObjectPool)
            {
                item.Value.MoveAllAlongPath(elaspedTime, pattern, backgroundSpeed, backgroundDirection);
            }
        }
        [Serializable]
        public class EnemySchedule : IScheduleItem
        {
            public bool flipOnX, flipOnY;
        }

        [Serializable]
        public class BackgroundSchedule : IScheduleItem
        {
            public int waitOnEnemyScheduleIndex; //0 will require no waiting- refers to organized scheule rather than serialized one
            public int stopSpawnOnXEnemiesDefeated;
            public float respawnTime, stopAtTimeStamp;
            public int repeatCount; // Ignored if looped
            public bool loop, randomizePosition;
        }

        public abstract class IScheduleItem
        {
            public int spawnOnXEnemiesDefeated;
            public float timeStamp;
            public bool active;
            public bool moveAlongPath;
            public int index;
            public float2 direction;
            public float2 spawnPosition;
        }

    }
}
