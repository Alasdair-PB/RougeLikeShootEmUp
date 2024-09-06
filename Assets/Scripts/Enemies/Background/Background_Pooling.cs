using Unity.Mathematics;
using UnityEngine;

public class Background_Pooling : MonoBehaviour
{
    private GameObject backgroundPrefab;
    private int initialPoolSize = 10;
    private int maxPoolSize = 999;

    private ObjectPool<BackgroundItem> backgroundPool;

    public void ClearPool() => backgroundPool.ReturnAllToPool();

    public Background_Pooling(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent)
    {
        backgroundPrefab = prefab;
        initialPoolSize = initialCapacity;
        maxPoolSize = maxCapacity;
        SetUp(parent);
    }

    private void SetUp(Transform transform)
    {
        backgroundPool = new ObjectPool<BackgroundItem>(backgroundPrefab.GetComponent<BackgroundItem>(), initialPoolSize, maxPoolSize, transform);
    }

    public void InstantiateBackground(float2 direction,float2 position)
    {
        BackgroundItem backgroundItem = backgroundPool.Get();
        SetUpBackgroundItem(backgroundItem, direction, position);
    }

    private void SetUpBackgroundItem(BackgroundItem backgroundItem, float2 direction, float2 position)
    {
        backgroundItem.transform.position = new Vector3(position.x, position.y, 0);
        backgroundItem.OnReturn = null;
        backgroundItem.OnReturn += ReturnBackgroundItem;
    }

    public void ReturnBackgroundItem(BackgroundItem backgroundItem)
    {
        backgroundPool.ReturnToPool(backgroundItem);
    }
}