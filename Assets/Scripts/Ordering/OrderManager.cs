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

    public Order CreateOrder(List<int> ingredientIndexes, int customerTypeId, Sprite lobbySprite, Sprite waitingSprite) {
        if (!HasCapacity()) {
            Debug.LogWarning("OrderManager: no capacity for new order.");
            return null;
        }

        var order = new Order(nextOrderId++);
        order.ingredientIndexes.AddRange(ingredientIndexes);
        order.customerTypeId = customerTypeId;
        order.lobbySprite = lobbySprite;
        order.waitingSprite = waitingSprite;

        activeOrders.Add(order.orderId, order);

        // Notify UI, etc.
        OrderUIManager.Instance?.OnOrderCreated(order);

        return order;
    }

    public Order GetOrder(int id) {
        activeOrders.TryGetValue(id, out var o);
        return o;
    }

    public void RemoveOrder(int id) {
        if (activeOrders.Remove(id)) {
            OrderUIManager.Instance?.OnOrderRemoved(id);
        }
    }

    public List<Order> GetAllOrders() {
        return new List<Order>(activeOrders.Values);
    }
}

