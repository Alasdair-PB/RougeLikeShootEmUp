using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalSaveData : MonoBehaviour
{
    private Dictionary<string, SaveData> localSaveDataPermanent = new Dictionary<string, SaveData>(); 
        //localSaveDataLossful= new Dictionary<string, SaveData>();
    private struct SaveData
    {
        public string id, data, location; 
    }  


    // Save data is cleared on scene change
    /*public string GetSaveDataLossful(string saveID, string saveFile)
    {
        if (localSaveDataLossful.ContainsKey(saveID))
        {
            return localSaveDataLossful[saveID].data;
        }
        return null;
    }*/

    // Save data is stored on Player Save Action
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
                localSaveDataPermanent[saveID] =  new SaveData { data = jsonData, id = saveID, location = saveFile };
                return jsonData;
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
        } else
        {
            localSaveDataPermanent[saveID] = new SaveData { data = newData, id = saveID, location = saveFile };
        }
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

    public void CreateDataPermanent(string saveID, string data, string saveFile) => SaveDataPermanent(saveID, data, saveFile);

    private void SaveDataPermanent(string saveID, string data, string saveFile)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFile);

        Dictionary<string, string> saveDataDict;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            saveDataDict = JsonUtility.FromJson<Dictionary<string, string>>(jsonData);
        }
        else
        {
            saveDataDict = new Dictionary<string, string>();
        }

        saveDataDict[saveID] = data;

        string updatedJsonData = JsonUtility.ToJson(saveDataDict, true);
        File.WriteAllText(path, updatedJsonData);
        localSaveDataPermanent[saveID] = new SaveData { data = data, id = saveID, location = saveFile };
    }

    public void SaveAllDataPermanent()
    {
        foreach (var saveEntry in localSaveDataPermanent)
        {
            SaveDataPermanent(saveEntry.Key, saveEntry.Value.data, saveEntry.Value.location);
        }
    }
}
