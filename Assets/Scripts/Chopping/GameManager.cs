using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int cutScore = 0;
    public int IngredientCounter;
    [SerializeField] private TextMeshProUGUI cutScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        cutScoreText.text = "Score: " + cutScore.ToString() + "/20";

    }

    public string GetScore()
    {
        return "Final Score\n" + cutScore.ToString() + "/20";
    }

    public void AddScore()
    {
        cutScore++;
        cutScoreText.text = "Score: " + cutScore.ToString() + "/20";
    }

    public void RemoveScore()
    {
        cutScore--;
        cutScoreText.text = "Score: " + cutScore.ToString() + "/20";
    }

    public void Reset() // Reloads the Scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddIngredient() // Used to count the total number of non tax folders spawned
    {
        IngredientCounter++;
    }

    public int GetIngredientCounter()
    {
        return IngredientCounter;
    }
}
