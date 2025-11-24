using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    [SerializeField] private string customerName;
    [SerializeField] private Sprite frontSprite, sideSprite;
    [SerializeField] private bool isIRSAgent;
    [SerializeField] private int waitTime = 20;
    [SerializeField] private float tipAmount = 5f;

    // Public getters for private fields
    public string CustomerName => customerName;
    public Sprite FrontSprite => frontSprite;
    public Sprite SideSprite => sideSprite;
    public bool IsIRSAgent => isIRSAgent;
    public int WaitTime => waitTime;
    public float TipAmount => tipAmount;
}
