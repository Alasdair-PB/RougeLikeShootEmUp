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
        this.parent = parent;
        this.capacity = maxCapacity;

        for (int i = 0; i < initialCapacity; i++)
        {
            T instance = Object.Instantiate(prefab, parent);
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

    public void ReturnAllToPool()
    {
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