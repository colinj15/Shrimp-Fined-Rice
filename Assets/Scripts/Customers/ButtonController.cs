using UnityEngine;

public class CustomerButtonController : MonoBehaviour
{

    public CustomerController customer;
    public Sprite[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnButtonClick(){
        var queueManager = FindAnyObjectByType<CustomerQueueManager>();
        var front = queueManager.PeekFrontCustomer();

        //only allow front customer
        if (customer != front) return;

        // generate order
        customer.order.GenerateOrder();

        // save globally
        Sprite currentSprite = customer.GetComponent<SpriteRenderer>()?.sprite;
        OrderSystem.AddOrder(customer.getName(), customer.order.Ingredients, currentSprite);

        queueManager.DequeueCustomer();
        queueManager.TrySpawnCustomer();
    }
}
