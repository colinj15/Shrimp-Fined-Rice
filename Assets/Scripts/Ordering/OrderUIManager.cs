using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderUIManager : MonoBehaviour {
    public static OrderUIManager Instance { get; private set; }

    [Header("UI")]
    public Transform ordersPanel; // parent where buttons are instantiated
    public GameObject orderButtonPrefab; // a UI button prefab

    private Dictionary<int, GameObject> orderButtons = new Dictionary<int, GameObject>();
    private int selectedOrderId = -1;

    private void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void OnOrderCreated(Order order) {
        if (order == null) return;
        // create a UI element for the order
        GameObject b = Instantiate(orderButtonPrefab, ordersPanel);
        b.name = "OrderButton_" + order.orderId;
        var button = b.GetComponent<Button>();
        var text = b.GetComponentInChildren<Text>();
        if (text != null) {
            // build a short description using ingredient names (requires access to ingredient list)
            // assuming a singleton or reference is present. For now, show ID and ingredient count
            text.text = $"Order {order.orderId} ({order.ingredientIndexes.Count})";
        }

        // store mapping
        orderButtons.Add(order.orderId, b);

        // wire up click
        button.onClick.AddListener(() => OnOrderButtonClicked(order.orderId));
    }

    public void OnOrderRemoved(int orderId) {
        if (orderButtons.TryGetValue(orderId, out GameObject go)) {
            Destroy(go);
            orderButtons.Remove(orderId);
        }

        if (selectedOrderId == orderId) selectedOrderId = -1;
    }

    private void OnOrderButtonClicked(int orderId) {
        selectedOrderId = orderId;
        Debug.Log($"Selected order {orderId} for delivery.");
        // visually highlight selected button (not implemented in this sample)
    }

    public int GetSelectedOrderId() {
        return selectedOrderId;
    }

    public void ClearSelection() {
        selectedOrderId = -1;
        // remove highlight visually if implemented
    }
}

