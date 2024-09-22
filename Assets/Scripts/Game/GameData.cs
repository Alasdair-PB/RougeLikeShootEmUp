using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public SerializableDictionary<string, Vector3> positionData;
    public SerializableDictionary<string, string> stringData;
    public SerializableDictionary<string, float> floatData;
    public SerializableDictionary<string, int> intData;
    public SerializableDictionary<string, NPC_GameData> NPCData;

    public SerializableDictionary<string, SerializableList<int>> indexPairsData;


    public GameData()
    {
        positionData = new SerializableDictionary<string, Vector3>();
        stringData = new SerializableDictionary<string, string>();
        floatData = new SerializableDictionary<string, float>();
        indexPairsData = new SerializableDictionary<string, SerializableList<int>>();
        intData = new SerializableDictionary<string, int>();
        NPCData = new SerializableDictionary<string, NPC_GameData>();
    }

    public T GetValue<T>(string id)
    {
        if (typeof(T) == typeof(Vector3))
            return (T)(object)(positionData.ContainsKey(id) ? positionData[id] : positionData[id] = Vector3.zero);
        else if (typeof(T) == typeof(string))
            return (T)(object)(stringData.ContainsKey(id) ? stringData[id] : stringData[id] = string.Empty);
        else if (typeof(T) == typeof(float))
            return (T)(object)(floatData.ContainsKey(id) ? floatData[id] : floatData[id] = 0);
        else if (typeof(T) == typeof(int))
            return (T)(object)(intData.ContainsKey(id) ? intData[id] : intData[id] = 0);
        else if (typeof(T) == typeof(SerializableList<int>))
            return (T)(object)(floatData.ContainsKey(id) ? indexPairsData[id] : indexPairsData[id] = new SerializableList<int>() { 0, 0 });
        else if (typeof(T) == typeof(NPC_GameData))
            return (T)(object)(NPCData.ContainsKey(id) ? NPCData[id] : NPCData[id] = new NPC_GameData() { optional = 0, 
                termDateAtLastSave = 0, termOptional = 0, mainIndex = 0 });
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
        else if (typeof(T) == typeof(SerializableList<int>))
            indexPairsData[id] = (SerializableList<int>)(object)newValue;
        else if (typeof(T) == typeof(int))
             intData[id] = (int)(object)newValue;
        else if (typeof(T) == typeof(NPC_GameData))
            NPCData[id] = (NPC_GameData)(object)newValue;
    }
}

[Serializable]
public struct NPC_GameData
{
    public int termDateAtLastSave, mainIndex, termOptional, optional;

}
