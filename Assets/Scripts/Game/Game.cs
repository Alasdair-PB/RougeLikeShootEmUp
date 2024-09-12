using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public Action StartGame;
    public Action<bool> EndGame;
    public Action Reset; 

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

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
