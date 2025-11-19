using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public CustomerController customerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CustomerData.CurrentSprite != null) {
            customerController.changeSprite(CustomerData.CurrentSprite);
            customerController.setName(CustomerData.CurrentName);
        }
    }
}
