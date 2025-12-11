using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public TextMeshProUGUI requiredText;
    public TextMeshProUGUI timerText;
    public Canvas mainCanvas;
    public Canvas endScreenCanvas;
    public TextMeshProUGUI endScoreText;
    private Coroutine timerRoutine;
    private int remainingSeconds = 40;

    private Dictionary<string, int> requiredCounts = new Dictionary<string, int>();
    private Dictionary<string, int> satisfiedCounts = new Dictionary<string, int>();
    private bool requirementsSatisfied = false;
    private OrderSystem.OrderData activeOrder;
    public TextMeshProUGUI timerText;

    public float timeLimit = 10f;
    private float timeRemaining;
    private bool timeUp = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var veggie = collision.GetComponentInParent<VeggieController>();
        if (veggie == null)
            return;

        var normalized = NormalizeIngredientName(veggie.IngredientName);
        if (string.IsNullOrEmpty(normalized))
        {
            Destroy(veggie.gameObject);
            return;
        }

        if (requiredCounts.TryGetValue(normalized, out int needed))
        {
            int current = satisfiedCounts.TryGetValue(normalized, out int val) ? val : 0;
            if (current < needed) {
                satisfiedCounts[normalized] = current + 1;
                Debug.Log($"[BasketController] Clean veggie added: {normalized} ({satisfiedCounts[normalized]}/{needed})");
                UpdateRequiredText();
                CheckRequirementsCompletion();
            }
        }

        Destroy(veggie.gameObject);
    }

    void Start() {
        timeRemaining = timeLimit;
        timeUp = false;
    }


    void Start() {
        timeRemaining = timeLimit;
        timeUp = false;
    
        if (endScreenCanvas != null)
            endScreenCanvas.enabled = false;

        InitializeRequirements();
        StartTimer();
    }

    private void InitializeRequirements()
    {
        activeOrder = OrderSystem.ActiveMinigameOrder;
        requiredCounts.Clear();
        satisfiedCounts.Clear();
        requirementsSatisfied = false;

        if (activeOrder?.Ingredients == null || activeOrder.Ingredients.Count == 0)
        {
            Debug.Log("[BasketController] No active order ingredients; washing minigame has no specific requirements.");
            return;
        }

        foreach (var ingredient in activeOrder.Ingredients)
        {
            var normalized = NormalizeIngredientName(ingredient);
            if (string.IsNullOrWhiteSpace(normalized))
                continue;

            if (!requiredCounts.ContainsKey(normalized))
                requiredCounts[normalized] = UnityEngine.Random.Range(1, 4); // between 1 and 3 inclusive
        }

        foreach (var ingredient in requiredCounts.Keys)
            satisfiedCounts[ingredient] = 0;

        Debug.Log($"[BasketController] Washing requires clean veggies for order {activeOrder.CustomerID}: {string.Join(", ", activeOrder.Ingredients)}");
        UpdateRequiredText();
    }

    private string NormalizeIngredientName(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var normalized = input.Trim().ToLowerInvariant();

        // Basic plural handling: remove trailing 's' for singular match (but keep words ending with "ss")
        if (normalized.EndsWith("s") && normalized.Length > 1 && normalized[normalized.Length - 2] != 's')
            normalized = normalized.Substring(0, normalized.Length - 1);

        return normalized;
    }

    private void CheckRequirementsCompletion()
    {
        if (requirementsSatisfied || requiredCounts.Count == 0)
            return;

        foreach (var kvp in requiredCounts)
        {
            if (!satisfiedCounts.TryGetValue(kvp.Key, out int count) || count < kvp.Value)
                return;
        }

        requirementsSatisfied = true;
        OnRequirementsMet();
    }

    private void OnRequirementsMet()
    {
        if (activeOrder != null)
        {
            StopTimer();
            int bonus = remainingSeconds * 3;
            OrderManager.Instance?.AddScore(activeOrder.CustomerID, bonus);
            OrderManager.Instance?.MarkMinigameComplete(activeOrder.CustomerID, OrderManager.MinigameType.Washing);
            UpdateRequiredText();
            ShowEndScreen(bonus);
            Debug.Log($"[BasketController] Washing complete for order {activeOrder.CustomerID}. Time bonus: {bonus}");
        }
        else
        {
            Debug.Log("[BasketController] Washing requirements met but no active order tracked.");
        }
    }

    private void UpdateRequiredText()
    {
        if (requiredText == null)
        {
            Debug.LogWarning("[BasketController] requiredText is not assigned.");
            return;
        }

        var builder = new System.Text.StringBuilder();
        builder.AppendLine("Required:");

        bool allMet = true;
        foreach (var kvp in requiredCounts)
        {
            satisfiedCounts.TryGetValue(kvp.Key, out int current);
            int remaining = Mathf.Max(0, kvp.Value - current);
            builder.AppendLine($"{FormatIngredientName(kvp.Key)} x{remaining}");

            if (remaining > 0)
                allMet = false;
        }

        if (allMet)
            builder.AppendLine("- Complete -");

        requiredText.text = builder.ToString();
    }

    private string FormatIngredientName(string normalized)
    {
        if (string.IsNullOrWhiteSpace(normalized))
            return normalized;

        var parts = normalized.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (string.IsNullOrEmpty(parts[i])) continue;
            parts[i] = char.ToUpper(parts[i][0]) + (parts[i].Length > 1 ? parts[i].Substring(1) : string.Empty);
        }
        return string.Join(" ", parts);
    }

    private void StartTimer()
    {
        remainingSeconds = 40;
        UpdateTimerText();

        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(TimerCountdown());
    }

    private void StopTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (remainingSeconds > 0 && !requirementsSatisfied)
        {
            yield return new WaitForSeconds(1f);
            remainingSeconds--;
            UpdateTimerText();
        }

        if (remainingSeconds <= 0 && !requirementsSatisfied)
        {
            Debug.Log("[BasketController] Time's up!");
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = $"Time: {remainingSeconds}s";
    }

    private void ShowEndScreen(int score)
    {
        if (mainCanvas != null)
            mainCanvas.enabled = false;

        if (endScreenCanvas != null)
            endScreenCanvas.enabled = true;

        if (endScoreText != null)
            endScoreText.text = $"Score: {score}";
    }
}
