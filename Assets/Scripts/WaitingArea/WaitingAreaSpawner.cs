using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WaitingAreaSpawner : MonoBehaviour {
    public GameObject waitingCustomerPrefab;
    public Transform[] spawnPoints; // 6

    private void OnEnable() {
        // When waiting scene loads, spawn existing orders
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name != gameObject.scene.name) return;
        SpawnWaitingCustomers();
    }

    private void SpawnWaitingCustomers() {
        if (OrderManager.Instance == null) return;

        var orders = OrderManager.Instance.GetAllOrders();

        // spawn up to available spawn points
        int spawnIndex = 0;
        foreach (var order in orders) {
            if (spawnIndex >= spawnPoints.Length) break;
            Vector3 pos = spawnPoints[spawnIndex].position;
            GameObject go = Instantiate(waitingCustomerPrefab, pos, Quaternion.identity);
            var ctrl = go.GetComponent<WaitingCustomerController>();
            ctrl.Setup(order.orderId, order.waitingSprite);
            spawnIndex++;
        }
    }
}

