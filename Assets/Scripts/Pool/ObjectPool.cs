using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class ObjectPool<T> where T : Component
{
    private Stack<T> pool = new Stack<T>();

    private T prefab;
    private Transform parent;
    private int capacity;

    public ObjectPool(T prefab, int initialCapacity, int maxCapacity, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = new GameObject(prefab.name).transform;

        this.capacity = maxCapacity;

        for (int i = 0; i < initialCapacity; i++)
        {
            T instance = Object.Instantiate(prefab, this.parent);
            instance.gameObject.SetActive(false);
            pool.Push(instance);
        }
    }

    public T Get()
    {
        if (pool.Count > 0)
        {
            T instance = pool.Pop();
            instance.gameObject.SetActive(true);
            return instance;
        }
        else
        {
            return Object.Instantiate(prefab, parent);
        }
    }

    public T Get(float3 position)
    {
        if (pool.Count > 0)
        {
            T instance = pool.Pop();
            instance.gameObject.transform.position = position;
            instance.gameObject.SetActive(true);
            return instance;
        }
        else
        {
            T instance = Object.Instantiate(prefab, position, Quaternion.identity);
            instance.transform.SetParent(parent);
            return instance;
        }
    }

    public void ReturnToPool(T instance)
    {
        if (pool.Count < capacity)
        {
            instance.gameObject.SetActive(false);
            pool.Push(instance);
        }
        else
        {
            Object.Destroy(instance.gameObject);
        }
    }

    public void MoveAlongPath(T instance, float elapsedTime, PatternBase pattern, float speed, float2 direction)
    {
        float2 nextPos = new float2(instance.transform.position.x, instance.transform.position.y);
        nextPos = pattern.MoveInDirection(nextPos, direction, speed, elapsedTime);
        UpdatePosition(nextPos, instance.transform);
    }


    private void UpdatePosition(float2 nextPos, UnityEngine.Transform myTransform)
    {
        myTransform.position = new Vector3(nextPos.x, nextPos.y, myTransform.position.z);
    }

    public void MoveAllAlongPath(float elapsedTime, PatternBase pattern, float speed, float2 direction)
    {
        if (parent == null)
            return;

        T[] instances = parent == null ? Object.FindObjectsOfType<T>() : parent.GetComponentsInChildren<T>(true);

        foreach (T instance in instances)
        {
            MoveAlongPath(instance, elapsedTime, pattern, speed, direction);
        }
    }

    public T[] GetAllOfType()
    {
        if (parent == null)
            return null;

        T[] instances = parent == null ? Object.FindObjectsOfType<T>() : parent.GetComponentsInChildren<T>(true);
        return instances;
    }


    public void ReturnAllToPool()
    {
        if (parent == null)
            return;

        T[] instances = parent == null ? Object.FindObjectsOfType<T>() : parent.GetComponentsInChildren<T>(true);

        foreach (T instance in instances)
        {
            if (!pool.Contains(instance))
            {
                ReturnToPool(instance);
            }
        }
    }
}