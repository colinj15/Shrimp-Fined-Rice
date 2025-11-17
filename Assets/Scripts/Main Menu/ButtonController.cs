using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
   public void Play()
    {
        SceneManager.LoadScene("Kitchen");
    }
        
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    
    public void Back()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
