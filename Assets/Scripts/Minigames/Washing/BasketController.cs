using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    private int score = 0;
    private List<VeggieController> veggies = new List<VeggieController>();
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public float timeLimit = 10f;
    private float timeRemaining;
    private bool timeUp = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var veggie = collision.GetComponent<VeggieController>();
        veggies.Add(veggie);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var veggie = collision.GetComponent<VeggieController>();
        veggies.Remove(veggie);
    }

    void Start() {
        timeRemaining = timeLimit;
        timeUp = false;
    }


    // Update is called once per frame
    void Update()
    {
        timerText.text = $"Time: {timeRemaining:0.0}";
        // TIMER
        if (!timeUp) {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f) {
                timeUp = true;
                timeRemaining = 0f; 
            }
        }

        int orderId = OrderingOrderUIManager.Instance.GetSelectedOrderId();

        if (!timeUp) {
            score = 0;

            for (int i = 0; i < veggies.Count; i++) {
                var veggie = veggies[i];
                if (veggie == null) {
                    veggies.RemoveAt(i);  
                    i--;
                    continue;              
                }

                if (veggie.dirtyness <= 0f) {
                    bool correct = OrderManager.Instance.OrderContainsVeggie(orderId,veggie.veggieType);

                    if (correct) score += 1;
                    else score -= 1;
                }
            }
        }
        scoreText.text = "Score: " + score;
    }

    public int GetWeightedScore() {
        // Example weighted score: each clean veggie counts as 1, partial cleanliness contributes proportionally
        if (veggies == null || veggies.Count == 0) return 0;

        float total = 0f;
        int counted = 0;

        for (int i = 0; i < veggies.Count; i++) {
            var veggie = veggies[i];
            if (veggie == null) continue;

            // Assume dirtyness ranges 0 (clean) to 1 (fully dirty); weight inversely
            float cleanliness = Mathf.Clamp01(1f - veggie.dirtyness);
            total += cleanliness;
            counted++;
        }

        // Return rounded weighted score
        return Mathf.RoundToInt(total);
    }

    public void FinishWashingMinigame() {
        var order = OrderSystem.ActiveMinigameOrder;
        if (order != null) {

            int weighted = ScoreUtility.ToWeighted20(score, 20);

            OrderManager.Instance.AddScore(order.CustomerID, weighted, OrderManager.MinigameType.Washing);
            OrderManager.Instance.MarkMinigameComplete(order.CustomerID, OrderManager.MinigameType.Washing);
        }
    }
}
