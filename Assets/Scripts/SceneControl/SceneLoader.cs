using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {
    public void LoadLobby() {
        SceneManager.LoadScene("Lobby");
    }

    public void LoadWaitingArea() {
        SceneManager.LoadScene("WaitingArea");
    }

    public void LoadKitchen() {
        SceneManager.LoadScene("Kitchen");
    }
}

