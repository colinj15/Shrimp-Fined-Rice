using UnityEngine;
using UnityEngine.UI;

public class GameLink : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject bellButton;
    public GameObject posterButton;
    public GameObject panButton;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        UpdateButtons();
    }

    void Update()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        } else
        {
            UpdateButtons();
        }
    }

    public void UpdateButtons()
    {
        if (gameManager.GetHasDoorbell())
            {
                bellButton.SetActive(false);
            }
            if (gameManager.GetPostersBought() > 6)
            {
                posterButton.SetActive(false);
            }
            if (gameManager.GetHasStainlessPan())
            {
                panButton.SetActive(false);
            }
    }

    public void PurchaseUpgrade(int i)
    {
        if (gameManager.GetMoney() < i) return;
        switch (i)
        {
            case 10:
                gameManager.UnlockUpgrade("Doorbell");
            break;
            case 5:
                gameManager.UnlockUpgrade("Poster");
            break;
            case 15:
                gameManager.UnlockUpgrade("Stainless Pan");
            break;
            default:
                return;
        }
        gameManager.SubtractMoney(i);
    }
}
