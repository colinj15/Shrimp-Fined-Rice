using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonController : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Kitchen");
    }
}
