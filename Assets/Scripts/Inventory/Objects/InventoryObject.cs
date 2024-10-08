using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Inventory/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public InventoryObjectBase inventoryObject;
}


[Serializable]
public class InventoryObjectBase
{
    public string stringName, saveFile;
    public float maxCount, minCount;
    public bool descreaseOnlyOnFullCount;

    public bool CheckCanModify(float change, float itemCount)
    {
        if (change == 0)
            return true;

        if (descreaseOnlyOnFullCount && change < 0 && itemCount < change)
            return false;
        return true;

    }
}
