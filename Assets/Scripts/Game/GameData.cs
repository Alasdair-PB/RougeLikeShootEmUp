using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Dictionary<string, Vector3> positionData;
    public Dictionary<string, string> stringData;
    public Dictionary<string, float> floatData;


    public GameData()
    {
        positionData = new SerializableDictionary<string, Vector3>();
        stringData = new SerializableDictionary<string, string>();
        floatData = new SerializableDictionary<string, float>();
    }

    public T GetValue<T>(string id)
    {
        if (typeof(T) == typeof(Vector3))
            return (T)(object)(positionData.ContainsKey(id) ? positionData[id] : positionData[id] = Vector3.zero);
        else if (typeof(T) == typeof(string))
            return (T)(object)(stringData.ContainsKey(id) ? stringData[id] : stringData[id] = string.Empty);
        else if (typeof(T) == typeof(float))
            return (T)(object)(floatData.ContainsKey(id) ? floatData[id] : floatData[id] = 0);
        else
            return default(T);
    }

    public void UpdateValue<T>(string id, T newValue)
    {
        if (typeof(T) == typeof(Vector3))
            positionData[id] = (Vector3)(object)newValue;
        else if (typeof(T) == typeof(string))
            stringData[id] = (string)(object)newValue;
        else if (typeof(T) == typeof(float))
            floatData[id] = (float)(object)newValue;
    }
}
