using UnityEngine;

public static class OrderRemovalHelper {
    public static void RemoveOrderCompletely(int orderId) {
        Order order = OrderManager.Instance.GetOrder(orderId);
        if (order == null) {
            Debug.LogWarning("tried to shred non-existant ticket");
            return;
        }
        RemoveCustomer(order);

        OrderManager.Instance.RemoveOrder(orderId);

        Debug.Log($"Order {orderId} and corresponding cust removed");
    }

    private static void RemoveCustomer(Order order) {
        CustomerController customer = GameObject.FindObjectOfType<CustomerController>();

        foreach (var ctrl in GameObject.FindObjectsOfType<CustomerController()) {
            if (ctrl.typeId == order.customerTypeId) {
                LobbySpawnManager l = GameObject.FindObjectOfType<LobbySpawnManager>();
                if (l != null) l.FreeSpawnPoint(ctrl.currentSpawnIndex);
                
                GameObject.Destroy(ctrl.gameObject);
                return;
            }
        }
    }

}