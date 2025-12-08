using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order {
    public int orderId; // unique
    public List<int> ingredientIndexes = new List<int>(); // indexes into IngredientListSO
    public int customerTypeId; // optional: which customer sprite/type to use
    public Sprite lobbySprite;
    public Sprite waitingSprite;
    // You can add other metadata like timeOrdered, patience, etc.

    public Order(int id) {
        orderId = id;
    }
}
