using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbySpawnManager : MonoBehaviour {
    public GameObject customerPrefab; // set in inspector
    public Transform[] spawnPoints;   // 6 spawn points
    public float minSpawnDelay = 2.0f;
    public float maxSpawnDelay = 6.0f;

    [System.Serializable]
    public struct CustomerType {
        public string name;
        public Sprite lobbySprite;
        public Sprite waitingSprite;
    }
    public CustomerType[] customerTypes;

    private Queue<int> availableSpawns;

    private void Start() {
        // validate inspector fields
        if (customerPrefab == null) Debug.LogError("LobbySpawnManager: customerPrefab not set.");
        if (spawnPoints == null || spawnPoints.Length == 0) Debug.LogError("LobbySpawnManager: spawnPoints not set.");

        availableSpawns = new Queue<int>();
        for (int i = 0; i< spawnPoints.Length; i++) {
            availableSpawns.Enqueue(i);
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        yield return new WaitForSeconds(1f); // initial pause

        while (true) {
            // stop spawning customers if daily limit reached
            if (DailyCustomerLimit.Instance != null && DailyCustomerLimit.Instance.DayIsOver) {
                Debug.Log("LobbySpawnManager: daily customer limit reached- no more customers will spawn");
                yield break;
            }

            // wait until there's capacity for a new order (max 6 active orders)
            if (OrderManager.Instance == null) {
                yield return null;
                continue;
            }

            if (!OrderManager.Instance.HasCapacity()) {
                // can't create more orders now
                yield return new WaitForSeconds(1f);
                continue;
            }

            // pick a random delay
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // pick a spawn point that is currently free (spawn manager does not track customers removed after ordering;
            // to avoid overlap we'll just check whether the spawn point has a collider overlap or maintain a local occupancy list.)
            // Simple approach: allow spawn if there is no child at that spawn position (better: we could keep track).
            int spawnIndex = ChooseFreeSpawnIndex();
            if (spawnIndex == -1) {
                // none free, skip this cycle
                yield return new WaitForSeconds(1f);
                continue;
            }

            Transform spawnPoint = spawnPoints[spawnIndex];

            // pick a random customer type
            int typeIndex = 0;
            if (customerTypes != null && customerTypes.Length > 0) {
                typeIndex = Random.Range(0, customerTypes.Length);
            }

            GameObject go = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            var ctrl = go.GetComponent<CustomerController>();
            if (ctrl != null) {
                var type = customerTypes[typeIndex];
                ctrl.Setup(typeIndex, type.name, type.lobbySprite, type.waitingSprite);
            }
        }
    }

    private int ChooseFreeSpawnIndex() {
    if (availableSpawns.Count == 0)
        return -1; // no free spots
    return availableSpawns.Dequeue(); // take first free index
    }


    public void FreeSpawnPoint(int spawnIndex) {
    availableSpawns.Enqueue(spawnIndex);
    }

}
