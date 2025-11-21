using System.Collections.Generic;
using UnityEngine;

public class WaitingAreaManager : MonoBehaviour {
    public static WaitingAreaManager Instance;

    public static OrderSystem.OrderData SelectedOrder;

    public Transform[] waitingSpots;
    public GameObject customerPrefab;

    private List<GameObject> waitingCustomers = new List<GameObject>();

    void Awake() {
        Instance = this;
    }

    void OnEnable() {
        OrderSystem.OnOrdersUpdated += RefreshWaitingArea;
    }

    void OnDisable() {
        OrderSystem.OnOrdersUpdated -= RefreshWaitingArea;
    }

    void Start() {
        RefreshWaitingArea();
    }

    public void RefreshWaitingArea() {
        // clear old customers
        foreach (var c in waitingCustomers)
            Destroy(c);

        waitingCustomers.Clear();

        // spawn customers based on orders
        for (int i = 0; i < OrderSystem.ActiveOrders.Count; i++) {
            if (i >= waitingSpots.Length)
                break;

            var order = OrderSystem.ActiveOrders[i];
            GameObject c = Instantiate(customerPrefab);

            var identity = c.GetComponent<CustomerIdentity>();
            identity.CustomerID = order.CustomerID;
            identity.CustomerName = order.CustomerName;
            identity.LobbySprite = order.LobbySprite;
            identity.WaitingSprite = order.WaitingSprite;

            identity.SetWaitingSprite();

            c.transform.position = waitingSpots[i].position;

            waitingCustomers.Add(c);
        }
    }

    public void RemoveWaitingCustomer(GameObject customer) {
        waitingCustomers.Remove(customer);
        Destroy(customer);

        RefreshWaitingArea();
    }
}

