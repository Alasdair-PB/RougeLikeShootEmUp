using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "ContinuousPrefabGroup", menuName = "PrefabGroup/ContinuousPrefabGroup")]
public class ContinuousPrefabGroup : PrefabGroupSO
{
    public ContinuousPrefab[] group;
    public override PrefabGroup[] GetPrefabGroup() => group;
    public  ContinuousPrefab[] GetContPrefabGroup() => group;

}


[Serializable]
public class ContinuousPrefab : PrefabGroup
{
    public int minCount, maxCount;
    public float2 minPos, maxPos;
}
