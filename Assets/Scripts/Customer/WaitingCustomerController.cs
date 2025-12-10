using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaitingCustomerController : MonoBehaviour {
    private int orderId;
    private SpriteRenderer sr;

    public int dailyCustomerCount;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Setup(int orderId, Sprite waitingSprite) {
        this.orderId = orderId;
        if (sr != null && waitingSprite != null)
            sr.sprite = waitingSprite;
    }

    private void OnMouseDown() {
        // Player clicked this waiting customer; we must check if an order is selected
        OrderUIManager ui = OrderUIManager.Instance;
        if (ui == null) return;

        int selectedOrderId = ui.GetSelectedOrderId();
        if (selectedOrderId == -1) {
            // no order selected: maybe show hint "select an order first"
            Debug.Log("No order selected. Click an order in the UI then click the customer.");
            return;
        }

        if (selectedOrderId == orderId) {
            // deliver success
            Debug.Log($"Delivered order {orderId}!");
            OrderManager.Instance.RemoveOrder(orderId);
            Destroy(gameObject);
            ui.ClearSelection();
            dailyCustomerCount ++;

        } else {
            // wrong order delivered
            Debug.Log($"This customer does not have order {selectedOrderId}. Try again.");
            // optionally give a penalty or feedback
        }
    }

    public int GetDailyCustomerCount() {
        return dailyCustomerCount;
    }
}

