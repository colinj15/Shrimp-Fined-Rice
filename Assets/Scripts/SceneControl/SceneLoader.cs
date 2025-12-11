using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void LoadLobby()
    {
        StartCoroutine(LoadLobbySequence());
    }

    private IEnumerator LoadLobbySequence()
    {
        if (!gameManager.GetDayInProgress())
        {
            gameManager.SetDayInProgress(true);
            yield return StartCoroutine(gameManager.DayCountdown());
        }

        SceneManager.LoadScene("Lobby");
    }

    public void LoadWaitingArea()
    {
        SceneManager.LoadScene("WaitingArea");
    }

    public void LoadKitchen()
    {
        SceneManager.LoadScene("Kitchen");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

