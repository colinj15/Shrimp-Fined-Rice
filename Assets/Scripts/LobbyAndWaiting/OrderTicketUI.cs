using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderTicketUI : MonoBehaviour {
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ingredientsText;
    public Image highlightImage;
    public Button button;

    private OrderSystem.OrderData order;

    // called by OrderUIManager when ticket is created
    public void Initialize(OrderSystem.OrderData orderData) {
        order = orderData;

        avatarImage.sprite = order.WaitingSprite;
        nameText.text = order.CustomerName;
        ingredientsText.text = string.Join(", ", order.Ingredients);

        button.onClick.AddListener(OnTicketClicked);
    }

    private void OnTicketClicked() {
        OrderUIManager.Instance.SelectTicket(this, order);
    }

    public void SetHighlight(bool enabled) {
        highlightImage.gameObject.SetActive(enabled);
    }

    public OrderSystem.OrderData GetOrder() => order;
}

