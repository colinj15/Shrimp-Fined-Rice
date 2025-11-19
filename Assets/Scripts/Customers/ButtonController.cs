using UnityEngine;

public class CustomerButtonController : MonoBehaviour
{

    public CustomerController customer;
    public Sprite[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnButtonClick()
    {
        customer.changeSprite(sprites[Random.Range(0, sprites.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
