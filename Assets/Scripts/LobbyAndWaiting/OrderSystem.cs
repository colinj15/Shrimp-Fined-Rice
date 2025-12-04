using System;
using System.Collections.Generic;
using System.Diagnostics;

public static class OrderSystem {
    public const int MaxOrders = 6;

    public static List<OrderData> ActiveOrders = new List<OrderData>();
    public static event Action OnOrdersUpdated;

    // a single order
    public class OrderData {
        public int CustomerID;
        public string CustomerName;
        public List<string> Ingredients;
        public UnityEngine.Sprite LobbySprite;       // side-facing
        public UnityEngine.Sprite WaitingSprite;     // front-facing

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
        UnityEngine.Sprite lobbySprite,
        UnityEngine.Sprite waitingSprite)
    {
        if (ActiveOrders.Count >= MaxOrders)
            return false;

        OrderData newOrder = new OrderData() {
            CustomerID = customerID,
            CustomerName = customerName,
            Ingredients = ingredients,
            LobbySprite = lobbySprite,
            WaitingSprite = waitingSprite
        };

        UnityEngine.Debug.Log($"[OrderSystem] Added order -> {newOrder}");

        ActiveOrders.Add(newOrder);
        OnOrdersUpdated?.Invoke();
        return true;
    }

    // Removes an order by reference
    public static void RemoveOrder(OrderData order) {
        if (ActiveOrders.Contains(order)) {
            ActiveOrders.Remove(order);
            OnOrdersUpdated?.Invoke();
        }
    }

    // Removes an order by ID
    public static void RemoveOrderByID(int customerID) {
        var match = ActiveOrders.Find(o => o.CustomerID == customerID);
        if (match != null)
        {
            ActiveOrders.Remove(match);
            OnOrdersUpdated?.Invoke();
        }
    }

    // Find specific customer's order
    public static OrderData GetOrderByID(int customerID) {
        return ActiveOrders.Find(o => o.CustomerID == customerID);
    }
}
