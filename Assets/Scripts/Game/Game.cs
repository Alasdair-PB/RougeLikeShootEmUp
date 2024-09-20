using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(LocalSaveData))]
public class Game : MonoBehaviour
{
    public Action StartGame;
    public Action<bool> EndGame;
    public Action Reset;

    private LocalSaveData my_localSaveData;


    private void Awake()
    {
        my_localSaveData = GetComponent<LocalSaveData>();
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
        StartCoroutine("DelayedRestart");
    }

    private IEnumerator DelayedRestart()
    {        
        yield return new WaitForSeconds(3);
        Reset?.Invoke();
        yield return new WaitForSeconds(1);
        StartGame?.Invoke();

    }

    public void UpdateSavedValue(string itemId, string saveFile, string newValue)
    {
        my_localSaveData.UpdateDataPermanent(itemId, saveFile, newValue);
    }

    public string GetSavedData(string itemID, string saveFile, string newValueOnUnInitialized)
    {
        var itemCount = my_localSaveData.GetSaveDataPermanent(itemID, saveFile);

        if (itemCount == null)
        {
            my_localSaveData.CreateDataPermanent(itemID, newValueOnUnInitialized, saveFile);
            itemCount = newValueOnUnInitialized;
        }

        return  itemCount;
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
