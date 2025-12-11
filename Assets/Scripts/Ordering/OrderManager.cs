using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour {
    public static OrderManager Instance { get; private set; }

    // Active orders (max 6). Keyed by orderId for quick lookup.
    private Dictionary<int, Order> activeOrders = new Dictionary<int, Order>();
    private int nextOrderId = 1;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public IReadOnlyCollection<Order> ActiveOrders => activeOrders.Values;

    public int ActiveCount => activeOrders.Count;

    public bool HasCapacity() => activeOrders.Count < 6;

    public Order CreateOrder(List<int> ingredientIndexes, List<string> ingredientNames, int customerTypeId, string customerName, Sprite lobbySprite, Sprite waitingSprite) {
        if (!HasCapacity()) {
            Debug.LogWarning("OrderManager: no capacity for new order.");
            return null;
        }

        var order = new Order(nextOrderId++);
        order.ingredientIndexes.AddRange(ingredientIndexes);
        if (ingredientNames != null) order.ingredientNames.AddRange(ingredientNames);
        order.customerTypeId = customerTypeId;
        order.customerName = string.IsNullOrEmpty(customerName) ? $"Customer {customerTypeId}" : customerName;
        order.lobbySprite = lobbySprite;
        order.waitingSprite = waitingSprite;

        activeOrders.Add(order.orderId, order);

        // Sync with shared OrderSystem for UI/minigames
        OrderSystem.AddOrder(
            order.orderId, 
            order.customerName, 
            order.ingredientNames, 
            order.lobbySprite, 
            order.waitingSprite
        );
        order.orderData = OrderSystem.GetOrderByID(order.orderId);

        OrderingOrderUIManager.Instance?.OnOrderCreated(order);

        return order;
    }

    public Order GetOrder(int id) {
        activeOrders.TryGetValue(id, out var o);
        return o;
    }

    public void RemoveOrder(int id) {
        if (activeOrders.Remove(id)) {
            OrderSystem.RemoveOrderByID(id);
            OrderingOrderUIManager.Instance?.OnOrderRemoved(id);
        }
    }

    // Score tracking
    public void AddScore(int customerId, int amount, MinigameType type) {
        Order order = GetOrderByCustomerID(customerId);
        if (order == null) return;

        switch (type) {
            case MinigameType.Frying:
                order.fryingScore = amount;
                break;

            case MinigameType.Cooking:
                order.cookingScore = amount;
                break;

            case MinigameType.Chopping:
                order.choppingScore = amount;
                break;

            case MinigameType.Washing:
                order.washingScore = amount;
                break;
        }
    }



    public int GetScore(int orderId) {
        return activeOrders.TryGetValue(orderId, out var order)
            ? order.choppingScore + order.washingScore + order.cookingScore + order.fryingScore : 0;

    }

    public enum MinigameType { Chopping, Washing, Cooking, Frying }
    public MinigameType? CurrentMinigameContext { get; private set; }

    public void SetCurrentMinigameContext(MinigameType? type) {
        CurrentMinigameContext = type;
    }

    // Mark a minigame as completed for this order; if all are done, mark order complete.
    public void MarkMinigameComplete(int orderId, MinigameType type) {
        if (!activeOrders.TryGetValue(orderId, out var order)) return;

        switch (type) {
            case MinigameType.Chopping: order.choppingComplete = true; break;
            case MinigameType.Washing: order.washingComplete = true; break;
            case MinigameType.Cooking: order.cookingComplete = true; break;
            case MinigameType.Frying: order.fryingComplete = true; break;
        }

        CheckAndCompleteOrder(order);
    }

    private void CheckAndCompleteOrder(Order order) {
        if (order == null) return;

        bool allDone = order.choppingComplete && order.washingComplete && order.cookingComplete && order.fryingComplete;
        if (allDone) {
            Debug.Log($"[OrderManager] Order {order.orderId} now complete (all minigames).");
            OrderSystem.MarkOrderCompleteByID(order.orderId);
            if (order.orderData != null)
                order.orderData.IsComplete = true;
        }
    }

    public bool ShouldHideForCurrentMinigame(int orderId) {
        if (CurrentMinigameContext == null) return false;
        if (!activeOrders.TryGetValue(orderId, out var order)) return false;

        bool isComplete = order.orderData != null && order.orderData.IsComplete;
        bool completedThisMini = CurrentMinigameContext switch {
            MinigameType.Chopping => order.choppingComplete,
            MinigameType.Washing => order.washingComplete,
            MinigameType.Cooking => order.cookingComplete,
            MinigameType.Frying => order.fryingComplete,
            _ => false
        };

        Debug.Log($"[OrderManager] Hide check Order {orderId}: complete={isComplete}, chopping={order.choppingComplete}, washing={order.washingComplete}, cooking={order.cookingComplete}, frying={order.fryingComplete}, context={CurrentMinigameContext}, hide={(isComplete || completedThisMini)}");

        if (isComplete) return true;
        return completedThisMini;
    }

    public List<Order> GetAllOrders() {
        return new List<Order>(activeOrders.Values);
    }

    public bool OrderContainsVeggie(int orderId, CabinetController.Veggie type) {
        if (!activeOrders.TryGetValue(orderId, out var order)) return false;

        foreach (string ingredient in order.ingredientNames) {
            if (ingredient.Equals(type.ToString(), System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    public Order GetOrderByCustomerID(int customerID) {
        foreach (Order order in activeOrders.Values) {
            if (order.customerTypeId == customerID) { 
                return order;
            }
        }
        return null;
    }


}
