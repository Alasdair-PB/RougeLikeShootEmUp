using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Stack<T> pool = new Stack<T>();
    private Stack<T> childPool = new Stack<T>();

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
}

public class GlobalPooling : MonoBehaviour
{
    [SerializeField] private float2 yBounds = new float2(), xBounds = new float2();

    private Dictionary<GameObject, ProPooling> globalPool = new Dictionary<GameObject, ProPooling>();

    public ProPooling GetPool(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent = null)
    {
        if (!globalPool.ContainsKey(prefab))
        {
            ProPooling newPool = new ProPooling(prefab, initialCapacity, maxCapacity, xBounds, yBounds, parent);
            globalPool[prefab] = newPool;
        }
        return globalPool[prefab];
    }
}

public class ProPooling 
{
    private GameObject projectilePrefab;
    private int initialPoolSize = 10;
    private int maxPoolSize = 999;
    private float2 yBounds = new float2(), xBounds = new float2();


    private ObjectPool<Pro_Controller> projectilePool;

    public ProPooling(GameObject prefab, int initialCapacity, int maxCapacity, float2 newXBounds, float2 newYBounds, Transform parent)
    {
        projectilePrefab = prefab;
        initialPoolSize = initialCapacity;
        maxPoolSize = maxCapacity;
        xBounds = newXBounds;
        yBounds = newYBounds;
        SetUp(parent);
    }

    private void SetUp(Transform transform)
    {
        projectilePool = new ObjectPool<Pro_Controller>(projectilePrefab.GetComponent<Pro_Controller>(), initialPoolSize, maxPoolSize, transform);
    }

    public void InstantiateProjectile(float2 direction, LayerMask layerMask, float2 position)
    {
        Pro_Controller projectile = projectilePool.Get();
        SetUpProjectile(projectile, direction, layerMask, position);
    }

    private void SetUpProjectile(Pro_Controller projectile, float2 direction, LayerMask layerMask, float2 position)
    {
        projectile.transform.position = new Vector3(position.x, position.y, 0);
        projectile.SetBounds(xBounds, yBounds);
        projectile.OnReturn = null;
        projectile.OnReturn += ReturnProjectile;
        projectile.InitializePro(direction, layerMask);
    }

    public void ReturnProjectile(Pro_Controller projectile)
    {
        projectilePool.ReturnToPool(projectile);
    }
}
