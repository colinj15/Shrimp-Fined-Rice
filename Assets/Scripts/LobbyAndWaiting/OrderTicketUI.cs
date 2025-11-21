using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderTicketUI : MonoBehaviour {
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ingredientsText;
    public Image highlightImage;

    private OrderSystem.OrderData order;

    public void SetOrder(OrderSystem.OrderData data) {
        order = data;

        avatarImage.sprite = data.WaitingSprite;
        nameText.text = data.CustomerName;
        ingredientsText.text = string.Join(", ", data.Ingredients);
    }

    public void OnClick() {
        OrderUIManager.Instance.SelectTicket(order, this);
    }

    public void SetHighlight(bool on) {
        highlightImage.gameObject.SetActive(on);
    }
}

