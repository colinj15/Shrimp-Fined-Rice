using UnityEngine;

public class CustomerInstance
{
    private CustomerData data;
    public int WaitTime { get; private set; }
    public float TipAmount { get; private set; }

    // Constructor
    public CustomerInstance(CustomerData data)
    {
        this.data = data;
        WaitTime = data.WaitTime;
        TipAmount = data.TipAmount;
    }

    // Decreases customer's wait time which will effect tip amount
    public void DecreaseWaitTime()
    {
        if(WaitTime > 0)
        {
            WaitTime--;
        }
    }

    // Calculate the tip based on the score of the customer's order + customer's wait time
    public void CalculateTip(int orderScore)
    {
        
    }

    // Get access to CustomerData data
    public string CustomerName => data.CustomerName;
    public Sprite FrontSprite => data.FrontSprite;
    public Sprite SideSprite => data.SideSprite;
    public bool IsIRSAgent => data.IsIRSAgent;
}
