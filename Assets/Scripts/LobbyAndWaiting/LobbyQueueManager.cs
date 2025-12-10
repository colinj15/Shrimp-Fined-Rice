using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LobbyQueueManager : MonoBehaviour {
    public static LobbyQueueManager Instance;

    public Transform[] queuePositions;
    public GameObject customerPrefab;

    private List<GameObject> queue = new List<GameObject>();

    public int minCustomers;
    public int maxCustomers;
    private int spawnedCustomers = 0;

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;

    void Awake(){
        Instance = this;
    }

    void Start() {
        SpawnNewCustomerAtBack();
        UpdateCustomerPositions();
        UpdateClickability();

        StartCoroutine(AutoFillQueueLoop());
    }

    IEnumerator AutoFillQueueLoop() {
        yield return new WaitForSeconds(1f);

        while(true) {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // if queue full, do nothing
            if (queue.Count >= queuePositions.Length) continue;

            //otherwise spawn new cust
            SpawnNewCustomerAtBack();
            UpdateCustomerPositions();
            UpdateClickability();
        }
    }
/*
    void FillQueue() {
        while (queue.Count < queuePositions.Length) {
            SpawnNewCustomerAtBack();
        }

        UpdateCustomerPositions();
        UpdateClickability();
    }*/

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

        UpdateCustomerPositions();
        UpdateClickability();
    }

/*
    IEnumerator SpawnAfterDelay() {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(delay);

        SpawnNewCustomerAtBack();
        UpdateCustomerPositions();
        UpdateClickability();
    }*/
}
