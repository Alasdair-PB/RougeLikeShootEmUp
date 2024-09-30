using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }  

    public Action StartGame;
    public Action<bool> EndGame;
    public Action Reset;
    public Action OnGameDataUpdate;

    private FileDataHandler my_localSaveData;
    private GameData localData;
    private readonly string profileID = "TesterID", dataDirPath = "saveData", 
        dataFileName = "mySaveData", myTermSaveData = "termDate";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject); 
            return;
        }

        my_localSaveData = new FileDataHandler(dataDirPath, dataFileName, false);
        localData = my_localSaveData.Load(profileID);

        if (localData == null)
        {
            localData = new GameData();
            Debug.Log("was null");
        }

        my_localSaveData.Save(localData, profileID);
    }

    private void OnEnable()
    {
        EndGame += EndGameState;
    }

    private void OnDisable()
    {
        EndGame -= EndGameState;
    }

    private void EndGameState(bool playerVictory)
    {
        Debug.Log(playerVictory ? "Player victorious" : "Player Down");
        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(3);
        Reset?.Invoke();
        yield return new WaitForSeconds(1);
        StartGame?.Invoke();
    }

    public int GetTermDate() => GetSavedData<int>(myTermSaveData, "");
    public void UpdateSavedValue<T>(string itemId, string saveFile, T newValue) 
    {
        localData.UpdateValue(itemId, newValue);
        OnGameDataUpdate?.Invoke();
    }

    public void SaveGameState() => my_localSaveData.Save(localData, profileID);
    public T GetSavedData<T>(string itemID, string saveFile) => localData.GetValue<T>(itemID);

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
