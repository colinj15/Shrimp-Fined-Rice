using TMPro;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    public float seconds;
    public TextMeshProUGUI time;
    public Canvas mainCanvas;
    public Canvas endScreenCanvas;
    public TextMeshProUGUI endScoreText;
    private bool finished = false;
    public HeatController heatController;
    private float initialSeconds;
    void Start()
    {
        initialSeconds = seconds;
        if (endScreenCanvas != null)
            endScreenCanvas.enabled = false;
        if (heatController != null)
            heatController.cookManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished && seconds > 0)
        {
            // Decrease once per second
            seconds -= Time.deltaTime;

            // Ensure time doesn’t go negative
            if (seconds <= 0)
            {
                seconds = 0;
                OnTimeUp();
            }
        }

        // Update time display
        time.text = Mathf.CeilToInt(seconds).ToString();
    }

    public bool IsTimeUp()
    {
        return seconds <= 0;
    }

    public int GetTime()
    {
        return Mathf.CeilToInt(seconds);
    }

    public float GetTotalTime() {
        return initialSeconds;
    }

    private void OnTimeUp()
    {
        if (finished) return;
        finished = true;

        if (mainCanvas != null)
            mainCanvas.enabled = false;

        if (endScreenCanvas != null)
            endScreenCanvas.enabled = true;

        int score = heatController != null ? heatController.GetDisplayScore() : Mathf.CeilToInt(seconds);
        if (endScoreText != null)
            endScoreText.text = $"Score: {score}";

        if (heatController != null && heatController.scoreText != null)
            heatController.scoreText.text = $"Score: {score}";

        var activeOrder = OrderSystem.ActiveMinigameOrder;
        if (activeOrder != null)
        {
            OrderManager.Instance?.AddScore(activeOrder.CustomerID, score, OrderManager.MinigameType.Cooking);
            OrderManager.Instance?.MarkMinigameComplete(activeOrder.CustomerID, OrderManager.MinigameType.Cooking);
        }
    }
}
