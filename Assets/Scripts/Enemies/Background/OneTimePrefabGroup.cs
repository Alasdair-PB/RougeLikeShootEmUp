using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "OneTimePrefabGroup", menuName = "PrefabGroup/OneTimePrefabGroup")]
public class OneTimePrefabGroup : PrefabGroupSO
{
    public OneTimePrefab[] group;
    public override PrefabGroup[] GetPrefabGroup() => group;
}


[Serializable]
public class OneTimePrefab : PrefabGroup
{
}
