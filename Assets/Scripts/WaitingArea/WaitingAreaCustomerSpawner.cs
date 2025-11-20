using UnityEngine;

public class WaitingAreaCustomerController : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnCustomersFromOrders();
    }

    void SpawnCustomersFromOrders() {
        int count = Mathf.Min(OrderSystem.ActiveOrders.Count, spawnPoints.Length);

        for (int i = 0; i < count; i++) {
            var ticket = OrderSystem.ActiveOrders[i];
            GameObject customerObj = Instantiate(customerPrefab, spawnPoints[i].position, Quaternion.identity);
            CustomerController controller = customerObj.GetComponent<CustomerController>();

            // Use saved sprite if available; otherwise you can leave default or set a fallback
            if (ticket.CustomerSprite != null) {
                controller.changeSprite(ticket.CustomerSprite);
            }

            controller.setName(ticket.CustomerName);

            // attach/order data to waiting-area customer so ticket matches customer
            var orderComp = customerObj.GetComponent<CustomerOrder>();
            if (orderComp == null) orderComp = customerObj.AddComponent<CustomerOrder>();
            orderComp.Init(controller);
            // copy ticket ingredients into the customer order so the waiting area knows what they wanted
            orderComp.SetIngredients(new System.Collections.Generic.List<string>(ticket.Ingredients));
        }
    }
}
