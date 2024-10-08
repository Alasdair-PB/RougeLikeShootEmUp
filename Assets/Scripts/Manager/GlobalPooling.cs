using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class GlobalPooling : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private float2 yBounds = new float2(), xBounds = new float2();

    private Dictionary<GameObject, ProPooling> globalPool = new Dictionary<GameObject, ProPooling>();

    public ProPooling GetProjectilePool(GameObject prefab, int initialCapacity, int maxCapacity, Transform parent = null)
    {
        if (!globalPool.ContainsKey(prefab))
        {
            ProPooling newPool = new ProPooling(prefab, initialCapacity, maxCapacity, xBounds, yBounds, parent);
            globalPool[prefab] = newPool;
        }
        return globalPool[prefab];
    }

    private void OnEnable()
    {
        game.Reset += ClearAllPools;
    }

    private void OnDisable()
    {
        game.Reset -= ClearAllPools;
    }

    private void ClearAllPools()
    {
        foreach (var item in globalPool)
        {
            item.Value.ClearPool();
        }
    }

}



