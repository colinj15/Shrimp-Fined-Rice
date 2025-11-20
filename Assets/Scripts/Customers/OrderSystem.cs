using UnityEngine;
using System.Collections.Generic;

public class OrderSystem : MonoBehaviour
{
    public static List<OrderTicket> ActiveOrders = new List<OrderTicket>();
    private const int MAX_ORDERS = 6;

    public static bool AddOrder(string customerName, List<string> ingredients, Sprite customerSprite = null) {
        if (ActiveOrders.Count >= MAX_ORDERS) return false;

        ActiveOrders.Add(new OrderTicket(customerName, ingredients, customerSprite));
        OrderUIManager.UpdateUI();
        return true;
    }

    public static void RemoveOrder(OrderTicket order) {
        ActiveOrders.Remove(order);
        OrderUIManager.UpdateUI();
    }
}


public class OrderTicket {
    public string CustomerName;
    public List<string> Ingredients;
    public Sprite CustomerSprite; // used by waiting area spawner

    public OrderTicket(string name, List<string> items, Sprite sprite = null) {
        CustomerName = name;
        Ingredients = new List<string>(items);
        CustomerSprite = sprite;
    }
}
