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

        [SerializeField] SingleScheduleGroup[] schedule;
        [SerializeField] ContinousScheduleGroup[] backgroundSchedule;
        private int[] activeBackgroundCount; 

        [SerializeField] Game game; 
        [SerializeField] float2 xBounds, yBounds, xBoundBackground, yBoundsBackground;
        [SerializeField] PatternBase pattern;
        [SerializeField] float backgroundSpeed;
        [SerializeField] float2 backgroundDirection;

        private int index, backgroundIndex, backgroundCheckIndex, enemyCount, enemiesDestroyed;
        private float startTime, pauseTime, bPauseTime, bStartTime, elaspedTime, nextSpawnTime, lastSpawnTime;
        private System.Random random;

        private bool beginSpawn = false;
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
            backgroundIndex = -1;
            backgroundCheckIndex = 0;
            pauseTime = 0;
            bPauseTime = 0;
            startTime = Time.time;
            bStartTime = Time.time;
            beginSpawn = false;
            nextSpawnTime = 0;
            lastSpawnTime = 0;
            elaspedTime = 0;
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
                {
                    for (int j = 0; j < schedule[i].group.GetPrefabGroup().Length; j ++) 
                    {
                        count++;
                    }
                }
                   
            }
            return count;
        }

        private void FixedUpdate()
        {
            if (index <= schedule.Length - 1 && UpdateItemSchedule(Time.time - startTime, 
                ref pauseTime, ref startTime, ref index, schedule, true))
            {
                SpawnSingleObject(index, schedule, true);
                index++;
            }

            if (backgroundCheckIndex <= backgroundSchedule.Length - 1)
            {
                if (UpdateItemSchedule(Time.time - bStartTime, ref bPauseTime, ref bStartTime,
                    ref backgroundCheckIndex, backgroundSchedule, true))
                {
                    backgroundIndex++;
                    backgroundCheckIndex++;
                }
            }
            if (backgroundIndex <= backgroundSchedule.Length - 1) { 
                if (backgroundIndex >= 0)
                    SpawnContinuousObjects(backgroundIndex, backgroundSchedule, false);
            }

            UpdateActiveElementsOnPath();
        }

        public IObjectPools GetObjectPool(Dictionary<GameObject, IObjectPools> myObjectPool,GameObject prefab, int initialCapacity, 
            int maxCapacity, bool enemy, Transform parent = null)
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

        private bool UpdateItemSchedule(float time, ref float pauseTimer, ref float myStartTime, ref int indexVal, 
            IScheduleItem[] mySchedule, bool enemy)
        {
            if (!mySchedule[indexVal].active)
            {
                indexVal++;
                return false;
            }

            if (mySchedule[indexVal].timeStamp < time)
            {
                if (enemiesDestroyed >= mySchedule[indexVal].spawnOnXEnemiesDefeated)
                {
                    if (pauseTimer > 0)
                        myStartTime = (Time.time - pauseTimer) + myStartTime;
                    
                    pauseTimer = 0;
                    return true;
                }
                else if (!(pauseTimer > 0))
                    pauseTimer = Time.time;
            }

            return false;
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

        private void SpawnObject(bool enemy, PrefabGroup[] myGroup, int i, float2 positionModifier)
        {
            var info = myGroup[i];
            IObjectPools objectInPool;

            if (enemy)
                objectInPool = GetObjectPool(globalObjectPool, prefabs[info.index], 1, 999, enemy, this.transform);
            else
                objectInPool = GetObjectPool(globalPathedObjectPool, backgroundPrefabs[info.index], 1, 999, enemy, this.transform);
            objectInPool.InstantiateObject(info.direction, info.spawnPosition + positionModifier, info.flipOnX, info.flipOnY, this);
        }

        private void SpawnSingleObject(int indexVal, SingleScheduleGroup[] mySchedule, bool enemy)
        {
            var item = mySchedule[indexVal];
            var myGroup = mySchedule[indexVal].group.GetPrefabGroup();

            for (int i = 0; i < myGroup.Length; i ++)
            {
                SpawnObject(enemy, myGroup, i, float2.zero);
            }
        }

        private void InitializeRandomSeed(int seed)
        {
            random = new System.Random(seed);
        }

        private void SpawnContinuousObjects(int indexVal, ContinousScheduleGroup[] mySchedule, bool enemy)
        {
            var item = mySchedule[indexVal];

            if (Time.time - lastSpawnTime <= nextSpawnTime)
                return;

            InitializeRandomSeed((int)DateTime.Now.Ticks);
            nextSpawnTime = (float)random.NextDouble() * (item.maxSpawnWait - item.minSpawnWait) + item.minSpawnWait;
            lastSpawnTime = Time.time;
            var myGroup = item.group.GetPrefabGroup();
            var i = random.Next(0, myGroup.Length);

            var myGroupAsCont = item.group.GetContPrefabGroup()[i];

            var spawnModifierX = (float)random.NextDouble() * (myGroupAsCont.maxPos.x - myGroupAsCont.minPos.x) + myGroupAsCont.minPos.x;
            var spawnModifierY = (float)random.NextDouble() * (myGroupAsCont.maxPos.y - myGroupAsCont.minPos.y) + myGroupAsCont.minPos.y;

            SpawnObject(enemy, myGroup, i, new float2(spawnModifierX, spawnModifierY));
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
        public class SingleScheduleGroup : IScheduleItem
        {
            public OneTimePrefabGroup group;
        }

        [Serializable]
        public class ContinousScheduleGroup : IScheduleItem
        {
            public ContinuousPrefabGroup group;

            // Backgroup specific
            public bool stopOnTimeOut, stopOnEnemiesOut;
            public int stopSpawnOnXEnemiesDefeated, stopAtXTime, minSpawnWait, maxSpawnWait;
        }

        [Serializable]
        public abstract class IScheduleItem 
        {
            public int spawnOnXEnemiesDefeated;
            public float timeStamp;
            public bool active;
        }

    }
}
