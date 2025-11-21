using System.Collections.Generic;
using UnityEngine;

public class OrderUIManager : MonoBehaviour {
    public static OrderUIManager Instance;

    public Transform ticketContainer;
    public GameObject ticketPrefab;

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

    private void Start() {
        RefreshUI();
    }

    public void RefreshUI() {
        foreach (Transform child in ticketContainer)
            Destroy(child.gameObject);

        foreach (var order in OrderSystem.ActiveOrders) {
            var ticketObj = Instantiate(ticketPrefab, ticketContainer);
            var ticketUI = ticketObj.GetComponent<OrderTicketUI>();
            ticketUI.Initialize(order);
        }
    }

    public void SelectTicket(OrderTicketUI ticket, OrderSystem.OrderData order) {
        if (selectedTicket != null)
            selectedTicket.SetHighlight(false);

        selectedTicket = ticket;
        selectedTicket.SetHighlight(true);

        WaitingAreaManager.SelectedOrder = order;
    }
}

