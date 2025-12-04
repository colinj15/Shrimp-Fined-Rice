using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OrderTicketUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ingredientsText;
    public Image highlightImage;

    private OrderSystem.OrderData order;
    private bool isSelected;
    private bool isHovered;

    public void SetOrder(OrderSystem.OrderData data) {
        order = data;

        if (avatarImage == null || nameText == null || ingredientsText == null) {
            Debug.LogError("[OrderTicketUI] UI references are not assigned on the ticket prefab.");
            return;
        }

        avatarImage.sprite = data.WaitingSprite;
        nameText.text = data.CustomerName;
        ingredientsText.text = string.Join(", ", data.Ingredients);

        UpdateHighlightVisual();
    }

    public void OnClick() {
        Debug.Log($"[OrderTicketUI] Ticket clicked: {order?.CustomerName ?? "<null order>"}");
        OrderUIManager.Instance.SelectTicket(order, this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnClick();
    }

    public void SetHighlight(bool on) {
        isSelected = on;
        UpdateHighlightVisual();
    }

    public void ClearHighlightState() {
        isSelected = false;
        isHovered = false;
        UpdateHighlightVisual();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovered = true;
        UpdateHighlightVisual();
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
        UpdateHighlightVisual();
    }

    private void UpdateHighlightVisual() {
        if (highlightImage == null) return;
        // Show highlight when selected or hovered
        highlightImage.gameObject.SetActive(isSelected || isHovered);
    }
}
