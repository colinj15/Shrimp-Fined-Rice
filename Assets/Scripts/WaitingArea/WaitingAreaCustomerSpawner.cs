using UnityEngine;

public class WaitingAreaCustomerController : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnCustomers();
    }

    void SpawnCustomers()
    {
        int count = Mathf.Min(CustomerData.OrderedSprites.Count, spawnPoints.Length);

        for (int i = 0; i < count; i++)
        {
            GameObject customerObj = Instantiate(customerPrefab, spawnPoints[i].position, Quaternion.identity);
            CustomerController controller = customerObj.GetComponent<CustomerController>();
            controller.changeSprite(CustomerData.OrderedSprites[i]);
            controller.setName(CustomerData.OrderedNames[i]);
        }
    }
}

