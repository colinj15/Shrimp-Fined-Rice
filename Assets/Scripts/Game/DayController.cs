using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayController : MonoBehaviour
{
    private GameManager gameManager;
    public TextMeshProUGUI customersServedText;
    public TextMeshProUGUI serviceScoreText;
    public TextMeshProUGUI tipsText;
    public TextMeshProUGUI daysUntilPaydayText;
    public TextMeshProUGUI IRSAgentsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void updateDayInfo()
    {
        customersServedText.text = "Customers served: " + gameManager.GetCustomersServed().ToString();
        serviceScoreText.text = "Service Score: " + gameManager.GetSatisfaction().ToString();
        tipsText.text = "Tips: " + gameManager.GetTips().ToString();
        daysUntilPaydayText.text = "Days until payday: " + (gameManager.GetDay() % 3).ToString();
        IRSAgentsText.text = "IRS Agents Spotted: " + gameManager.GetIrsSpotted().ToString();
    }
}