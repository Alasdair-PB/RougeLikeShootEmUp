using Unity.Mathematics;
using UnityEngine;

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