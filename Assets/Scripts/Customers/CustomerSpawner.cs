using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject customerPrefab;
    private CustomerController currentCustomer;
    void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        GameObject customerObject = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        currentCustomer = customerObject.GetComponent<CustomerController>();
        Sprite sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        currentCustomer.changeSprite(sprite);
        currentCustomer.setName(Regex.Replace(sprite.name, @"^\s*(\S+).*$", "$1"));

        //saves globally
        CustomerData.OrderedSprites.Add(sprite);
        CustomerData.OrderedNames.Add(currentCustomer.getName());
    }
}
