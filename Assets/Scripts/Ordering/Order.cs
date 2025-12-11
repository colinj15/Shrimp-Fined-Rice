using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order {
    public int orderId; // unique
    public List<int> ingredientIndexes = new List<int>(); // indexes into IngredientListSO
    public List<string> ingredientNames = new List<string>(); // names aligned to OrderSystem
    public int customerTypeId; // optional: which customer sprite/type to use
    public string customerName;
    public Sprite lobbySprite;
    public Sprite waitingSprite;
    public OrderSystem.OrderData orderData; // link to shared OrderSystem entry
    
    public int choppingScore;
    public int washingScore;
    public int cookingScore;
    public int fryingScore;

    // Minigame completion flags
    public bool choppingComplete;
    public bool washingComplete;
    public bool cookingComplete;
    public bool fryingComplete;
    // You can add other metadata like timeOrdered, patience, etc.

    public Order(int id) {
        orderId = id;
    }
}
