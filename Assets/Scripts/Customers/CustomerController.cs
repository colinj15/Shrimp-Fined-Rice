using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomerController : MonoBehaviour {
    private SpriteRenderer sr;
    private int typeId;
    private string customerName;
    private Sprite lobbySprite;
    private Sprite waitingSprite;

    [HideInInspector] public int currentSpawnIndex;
    [HideInInspector] public LobbySpawnManager lobbyManager;

    // reference to global ingredient list in inspector
    public IngredientListSO ingredientList;

    private bool hasOrdered = false;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Setup(int customerTypeId, string name, Sprite lobby, Sprite waiting) {
        typeId = customerTypeId;
        customerName = name;
        lobbySprite = lobby;
        waitingSprite = waiting;

        if (sr != null && lobbySprite != null)
            sr.sprite = lobbySprite;
    }

    private void OnMouseDown() {
        if (DailyCustomerLimit.Instance.DayIsOver) return;
        
        // If user clicks this customer, generate order (only once)
        if (!hasOrdered && OrderManager.Instance != null && OrderManager.Instance.HasCapacity()) {
            hasOrdered = true;
            CreateOrderAndLeave();
        }
    }

    private void CreateOrderAndLeave() {
        // generate a random order of 2-5 ingredients (no duplicates)
        if (ingredientList == null || ingredientList.ingredients.Count == 0) {
            Debug.LogError("CustomerController: ingredientList not set or empty.");
            return;
        }

        int ingredientCount = Random.Range(2, 6); // 2-5 inclusive
        List<int> chosenIndexes = new List<int>();
        List<int> pool = new List<int>();
        for (int i = 0; i < ingredientList.ingredients.Count; i++) pool.Add(i);

        for (int i = 0; i < ingredientCount && pool.Count > 0; i++) {
            int idx = Random.Range(0, pool.Count);
            chosenIndexes.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        // create order in OrderManager
        // map indexes to ingredient names for OrderSystem
        var ingredientNames = new List<string>();
        foreach (var idx in chosenIndexes) {
            if (idx >= 0 && idx < ingredientList.ingredients.Count) {
                var ing = ingredientList.ingredients[idx];
                if (ing != null && !string.IsNullOrWhiteSpace(ing.ingredientName))
                    ingredientNames.Add(ing.ingredientName);
            }
        }

        Order newOrder = OrderManager.Instance.CreateOrder(
            chosenIndexes,
            ingredientNames,
            typeId,
            string.IsNullOrEmpty(customerName) ? $"Customer {typeId}" : customerName,
            lobbySprite,
            waitingSprite);
        if (newOrder != null) {
            // optionally play an animation or move off screen
            Destroy(gameObject); // remove lobby customer after ordering
        } else {
            // order not created (capacity full). allow to be clicked again later
            hasOrdered = false;
        }


    }

}
