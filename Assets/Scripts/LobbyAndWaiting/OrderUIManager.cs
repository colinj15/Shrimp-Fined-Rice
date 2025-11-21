using UnityEngine;

public class OrderUIManager : MonoBehaviour {
    public static OrderUIManager Instance;

    public GameObject ticketPrefab;
    public RectTransform[] ticketSlots;

    private OrderTicketUI selectedTicket;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnEnable() {
        OrderSystem.OnOrdersUpdated += RefreshUI;
    }

    void OnDisable() {
        OrderSystem.OnOrdersUpdated -= RefreshUI;
    }

    public void RefreshUI() {
        // clear slot children
        foreach (var slot in ticketSlots) {
            foreach (Transform child in slot)
                Destroy(child.gameObject);
        }

        // spawn new tickets
        for (int i = 0; i < OrderSystem.ActiveOrders.Count; i++) {
            var ticketObj = Instantiate(ticketPrefab, ticketSlots[i]);
            var ui = ticketObj.GetComponent<OrderTicketUI>();
            ui.SetOrder(OrderSystem.ActiveOrders[i]);
        }
    }

    public void SelectTicket(OrderSystem.OrderData order, OrderTicketUI ticketUI) {
        if (selectedTicket != null)
            selectedTicket.SetHighlight(false);

        selectedTicket = ticketUI;
        selectedTicket.SetHighlight(true);

        // tell WaitingAreaManager which order is selected
        WaitingAreaManager.SelectedOrder = order;
    }
}
