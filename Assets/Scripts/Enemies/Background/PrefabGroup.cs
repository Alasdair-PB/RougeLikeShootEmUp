using System;
using Unity.Mathematics;
using UnityEngine;


[Serializable]
public abstract class PrefabGroup
{
    public int index;
    public float2 direction, spawnPosition;
    public bool flipOnX, flipOnY, moveAlongPath;
}


public abstract class PrefabGroupSO : ScriptableObject
{
    public abstract PrefabGroup[] GetPrefabGroup();
}