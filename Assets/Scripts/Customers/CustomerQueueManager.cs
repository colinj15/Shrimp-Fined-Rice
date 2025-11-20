using UnityEngine;
using System.Collections.Generic;

public class CustomerQueueManager : MonoBehaviour
{

    public GameObject customerPrefab;
    public Transform[] queuePositions;
    public Sprite[] sprites;

    private readonly Queue<CustomerController> queue = new Queue<CustomerController>();
    private const int MAX_ORDERS = 6; //max number of active orders

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrySpawnCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        if (queue.Count < MAX_ORDERS) {
            TrySpawnCustomer();
        }
    }

    public void TrySpawnCustomer() {
        if (queue.Count >= MAX_ORDERS) return;

        GameObject obj = Instantiate(customerPrefab, queuePositions[queue.Count].position, Quaternion.identity);
        CustomerController controller = obj.GetComponent<CustomerController>();

        //random sprite
        Sprite sprite = sprites[Random.Range(0, sprites.Length)];
        controller.changeSprite(sprite);
        controller.setName(sprite.name);

        var order = obj.AddComponent<CustomerOrder>();
        order.Init(controller);

        queue.Enqueue(controller);

        UpdateQueuePositions();
    }

    public CustomerController PeekFrontCustomer() {
        if (queue.Count == 0) return null;
        return queue.Peek();
    }

    public CustomerController DequeueCustomer() {
        if (queue.Count ==0) return null;

        var c = queue.Dequeue();
        UpdateQueuePositions();
        return c;
    }

    private void UpdateQueuePositions() {
        int i = 0;
        foreach (var cust in queue) {
            cust.transform.position = queuePositions[i].position;
            i++;
        }
    }
}
