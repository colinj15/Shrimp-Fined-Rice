using System.Collections.Generic;
using UnityEngine;

public class LobbyQueueManager : MonoBehaviour {
    public static LobbyQueueManager Instance;

    public Transform[] queuePositions;
    public GameObject customerPrefab;

    private List<GameObject> queue = new List<GameObject>();

    public int minCustomers;
    public int maxCustomers;
    private int spawnedCustomers = 0;

    void Awake(){
        Instance = this;
    }

    void Start() {
        FillQueue();
    }

    void FillQueue() {
        while (queue.Count < queuePositions.Length) {
            SpawnNewCustomerAtBack();
        }

        UpdateCustomerPositions();
        UpdateClickability();
    }

    void SpawnNewCustomerAtBack() {
        // make new character (identity + sprites)
        var identity = CustomerDatabase.Instance.GetUniqueRandomCharacter();

        GameObject c = Instantiate(customerPrefab);
        c.GetComponent<CustomerIdentity>().Initialize(identity);

        queue.Add(c);
    }

    void UpdateCustomerPositions() {
        for (int i = 0; i < queue.Count; i++) {
            queue[i].transform.position = queuePositions[i].position;
        }
    }

    void UpdateClickability() {
        for (int i = 0; i < queue.Count; i++) {
            var clickHandler = queue[i].GetComponent<CustomerClickHandler>();

            // Only front customer should respond to clicks
            clickHandler.isFrontCustomer = i == 0;
        }
    }

    // used by CustomerClickHandler when front customer places an order
    public void OnFrontCustomerOrdered(GameObject customer) {
        queue.Remove(customer);
        Destroy(customer);

        SpawnNewCustomerAtBack();
        UpdateCustomerPositions();
        UpdateClickability();
    }
}
