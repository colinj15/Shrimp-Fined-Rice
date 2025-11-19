using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingAreaButtons : MonoBehaviour
{
    public void GoToKitchen() {
        SceneManager.LoadScene("Kitchen");
    }

    public void GoToOrdering() {
        SceneManager.LoadScene("Lobby");
    }
}
