using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;

public class LocalSaveData : MonoBehaviour
{

    private Dictionary<string, SaveData> localSaveDataPermanent = new Dictionary<string, SaveData>();

    private struct SaveData
    {
        public string id, data, location;
    }

    public string GetSaveDataPermanent(string saveID, string saveFile)
    {
        if (localSaveDataPermanent.ContainsKey(saveID))
        {
            return localSaveDataPermanent[saveID].data;
        }
        else
        {
            string path = Path.Combine(Application.persistentDataPath, saveFile);
            if (File.Exists(path))
            {
                string jsonData = GetSpecifiedData(saveID, saveFile);
                if (jsonData != null)
                {
                    localSaveDataPermanent[saveID] = new SaveData { data = jsonData, id = saveID, location = saveFile };
                    return jsonData;
                }
            }
            return null;
        }
    }

    public void UpdateDataPermanent(string saveID, string saveFile, string newData)
    {
        if (localSaveDataPermanent.ContainsKey(saveID))
        {
            SaveData tempSaveData = localSaveDataPermanent[saveID];
            tempSaveData.data = newData;
            localSaveDataPermanent[saveID] = tempSaveData;
        }
        else
        {
            localSaveDataPermanent[saveID] = new SaveData { data = newData, id = saveID, location = saveFile };
        }
        SaveDataPermanent(saveID, newData, saveFile); 
    }

    private string GetSpecifiedData(string saveID, string saveFile)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFile);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            Dictionary<string, string> saveDataDict = JsonUtility.FromJson<Dictionary<string, string>>(jsonData);
            if (saveDataDict != null && saveDataDict.ContainsKey(saveID))
            {
                return saveDataDict[saveID];
            }
        }
        return null;
    }

    private string GetPath(string saveFile)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFile);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)); 
            string dataToStore = JsonUtility.ToJson(path, true);   
            

        } catch (Exception e)
        {
            Debug.LogException(e);
        }
        return path;
    }

    public void CreateDataPermanent(string saveID, string data, string saveFile)
    {
        UpdateDataPermanent(saveID, saveFile, data);
    }

    private void SaveDataPermanent(string saveID, string data, string saveFile)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFile);

        Dictionary<string, string> saveDataDict;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            saveDataDict = JsonUtility.FromJson<Dictionary<string, string>>(jsonData) ?? new Dictionary<string, string>();
        }
        else
        {
            saveDataDict = new Dictionary<string, string>();
        }

        saveDataDict[saveID] = data;
        string updatedJsonData = JsonUtility.ToJson(saveDataDict, true);
        File.WriteAllText(path, updatedJsonData); 
    }

    public void SaveAllDataPermanent()
    {
        foreach (var key in localSaveDataPermanent.Keys.ToList())
        {
            SaveData saveEntry = localSaveDataPermanent[key];
            SaveDataPermanent(key, saveEntry.data, saveEntry.location);
        }
    }
}
