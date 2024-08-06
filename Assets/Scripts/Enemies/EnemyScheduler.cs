using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScheduler : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] EnemySchedule[] schedule;
    [SerializeField] float2 xBounds, yBounds;

    private int index; 
    private float startTime;

    private Dictionary<GameObject, EnemyPooling> globalObjectPool = new Dictionary<GameObject, EnemyPooling>();

    public EnemyPooling GetObjectPool(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent = null)
    {
        if (!globalObjectPool.ContainsKey(prefab))
        {
            EnemyPooling newPool = new EnemyPooling(prefab, initialCapacity, maxCapacity,parent);
            globalObjectPool[prefab] = newPool;
        }
        return globalObjectPool[prefab];
    }

    private void Start()
    {
        startTime = Time.time;

        // Organize schedule
    }

    private void FixedUpdate()
    {
        var time = Time.time - startTime;

        if (index > schedule.Length - 1)
            return;

        if (schedule[index].timeStamp < time)
        {
            SpawnEnemy();
            index++;
        }

    }

    private void SpawnEnemy()
    {
        var scheduledEnemy = schedule[index];
        var objectInPool = GetObjectPool(prefabs[scheduledEnemy.enemyIndex], 1, 999);
        objectInPool.InstantiateEnemy(scheduledEnemy.direction, scheduledEnemy.spawnPosition, xBounds, yBounds);
    }

    [Serializable]
    public struct EnemySchedule
    {
        public int enemyIndex;
        public float timeStamp;
        public float2 direction;
        public float2 spawnPosition;
    }

}
