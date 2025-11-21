using UnityEngine;

public class CustomerClickHandler : MonoBehaviour {
    public bool isFrontCustomer = false;
    private CustomerIdentity identity;

    void Awake() {
        identity = GetComponent<CustomerIdentity>();
    }

    void OnMouseDown() {
        // LOBBY behavior
        if (LobbyQueueManager.Instance != null) {
            if (isFrontCustomer)
                HandleLobbyClick();

            return;
        }

        // WAITING AREA behavior
        if (WaitingAreaManager.Instance != null) {
            HandleWaitingAreaClick();
            return;
        }
    }

    private void HandleLobbyClick() {
        // Generate a random order
        var ingredients = CustomerDatabase.Instance.GenerateRandomIngredients();

        OrderSystem.AddOrder(
            identity.CustomerID,
            identity.CustomerName,
            ingredients,
            identity.LobbySprite,
            identity.WaitingSprite
        );

        // tell lobby queue that the front customer ordered
        LobbyQueueManager.Instance.OnFrontCustomerOrdered(gameObject);
    }

    private void HandleWaitingAreaClick() {
        var selectedOrder = WaitingAreaManager.SelectedOrder;

        if (selectedOrder == null) return;

        // if customer matches order -> deliver
        if (selectedOrder.CustomerID == identity.CustomerID) {
            // remove order
            OrderSystem.RemoveOrder(selectedOrder);

            // remove customer
            WaitingAreaManager.Instance.RemoveWaitingCustomer(gameObject);
        }
    }
}

