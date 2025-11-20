using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance;
    public Text ordersText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public static void UpdateUI() {
        if (Instance == null) return;

        StringBuilder sb = new StringBuilder();
        foreach (var order in OrderSystem.ActiveOrders) {
            sb.AppendLine($"{order.CustomerName}: {string.Join(", ", order.Ingredients)}");
        }

        Instance.ordersText.text = sb.ToString();
    }
}
