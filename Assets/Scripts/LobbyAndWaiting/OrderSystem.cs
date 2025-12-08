using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour {
    public static OrderSystem Instance { get; private set; }

    public const int MaxOrders = 6;

    private readonly List<OrderData> activeOrders = new List<OrderData>();
    private static readonly List<OrderData> EmptyOrders = new List<OrderData>();
    public static List<OrderData> ActiveOrders => Instance != null ? Instance.activeOrders : EmptyOrders;
    public static OrderData ActiveMinigameOrder { get; private set; }
    public static event Action OnOrdersUpdated;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // a single order
    public class OrderData {
        public int CustomerID;
        public string CustomerName;
        public List<string> Ingredients;
        public Sprite LobbySprite;       // side-facing
        public Sprite WaitingSprite;     // front-facing
        public bool IsComplete;

        public override string ToString() {
            var ingredientList = Ingredients == null ? "<none>" : string.Join(", ", Ingredients);
            return $"ID: {CustomerID} | Name: {CustomerName} | Ingredients: {ingredientList}";
        }

        // Maintain backwards compatibility if anything still calls the old method
        public string toString() => ToString();
    }

    // adds an order if possible
    public static bool AddOrder(
        int customerID,
        string customerName,
        List<string> ingredients,
        Sprite lobbySprite,
        Sprite waitingSprite)
    {
        if (!EnsureInstance(nameof(AddOrder))) return false;
        return Instance.AddOrderInternal(customerID, customerName, ingredients, lobbySprite, waitingSprite);
    }

    // Removes an order by reference
    public static void RemoveOrder(OrderData order) {
        if (!EnsureInstance(nameof(RemoveOrder))) return;
        Instance.RemoveOrderInternal(order);
    }

    // Removes an order by ID
    public static void RemoveOrderByID(int customerID) {
        if (!EnsureInstance(nameof(RemoveOrderByID))) return;
        Instance.RemoveOrderByIDInternal(customerID);
    }

    // Find specific customer's order
    public static OrderData GetOrderByID(int customerID) {
        if (!EnsureInstance(nameof(GetOrderByID))) return null;
        return Instance.activeOrders.Find(o => o.CustomerID == customerID);
    }

    // Marks an order as complete and notifies listeners
    public static void MarkOrderComplete(OrderData order) {
        if (!EnsureInstance(nameof(MarkOrderComplete))) return;
        Instance.MarkOrderCompleteInternal(order);
    }

    public static void MarkOrderCompleteByID(int customerID) {
        if (!EnsureInstance(nameof(MarkOrderCompleteByID))) return;
        var match = Instance.activeOrders.Find(o => o.CustomerID == customerID);
        if (match != null)
            Instance.MarkOrderCompleteInternal(match);
    }

    // Sets which order is active for a minigame session
    public static void SetActiveMinigameOrder(OrderData order) {
        if (!EnsureInstance(nameof(SetActiveMinigameOrder))) return;
        ActiveMinigameOrder = order;
    }

    // Replace all orders at once (useful when restoring persistence)
    public static void SetOrders(List<OrderData> orders) {
        if (!EnsureInstance(nameof(SetOrders))) return;
        Instance.activeOrders.Clear();
        if (orders != null)
            Instance.activeOrders.AddRange(orders);
        OnOrdersUpdated?.Invoke();
    }

    private bool AddOrderInternal(
        int customerID,
        string customerName,
        List<string> ingredients,
        Sprite lobbySprite,
        Sprite waitingSprite)
    {
        if (activeOrders.Count >= MaxOrders)
            return false;

        OrderData newOrder = new OrderData() {
            CustomerID = customerID,
            CustomerName = customerName,
            Ingredients = ingredients,
            LobbySprite = lobbySprite,
            WaitingSprite = waitingSprite,
            IsComplete = false
        };

        Debug.Log($"[OrderSystem] Added order -> {newOrder}");

        activeOrders.Add(newOrder);
        OnOrdersUpdated?.Invoke();
        return true;
    }

    private void RemoveOrderInternal(OrderData order) {
        if (activeOrders.Contains(order)) {
            activeOrders.Remove(order);
            OnOrdersUpdated?.Invoke();
        }
    }

    private void RemoveOrderByIDInternal(int customerID) {
        var match = activeOrders.Find(o => o.CustomerID == customerID);
        if (match != null)
        {
            activeOrders.Remove(match);
            OnOrdersUpdated?.Invoke();
        }
    }

    private void MarkOrderCompleteInternal(OrderData order) {
        if (order == null) return;
        order.IsComplete = true;
        OnOrdersUpdated?.Invoke();
    }

    private static bool EnsureInstance(string caller) {
        if (Instance != null) return true;
        Debug.LogError($"[OrderSystem] No instance in scene when calling {caller}. Make sure an OrderSystem GameObject exists.");
        return false;
    }
}
