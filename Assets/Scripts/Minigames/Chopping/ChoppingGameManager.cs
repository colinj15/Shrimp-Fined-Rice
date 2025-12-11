using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChoppingGameManager : MonoBehaviour
{
    public static ChoppingGameManager Instance;
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

    public void LoadKitchen()
    {
        // Apply score to the active minigame order and mark chopping complete
        var order = OrderSystem.ActiveMinigameOrder;
        if (order != null) {
            int totalScore = cutScore * 5;
            OrderManager.Instance?.AddScore(order.CustomerID, totalScore);
            OrderManager.Instance?.MarkMinigameComplete(order.CustomerID, OrderManager.MinigameType.Chopping);
        }

        SceneManager.LoadScene("Kitchen");
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
