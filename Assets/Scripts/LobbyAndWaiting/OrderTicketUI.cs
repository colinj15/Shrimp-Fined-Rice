using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OrderTicketUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ingredientsText;
    public Image highlightHoverImage;
    public Image highlightCompleteImage;

    private OrderSystem.OrderData order;
    private bool isSelected;
    private bool isHovered;
    private bool allowHighlight = true;
    private bool isComplete = false;

    public void SetOrder(OrderSystem.OrderData data) {
        order = data;

        if (avatarImage == null || nameText == null || ingredientsText == null) {
            Debug.LogError("[OrderTicketUI] UI references are not assigned on the ticket prefab.");
            return;
        }

        avatarImage.sprite = data.WaitingSprite;
        nameText.text = data.CustomerName;
        ingredientsText.text = string.Join(", ", data.Ingredients);
        isComplete = data.IsComplete;

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

    public void DisableHighlighting() {
        allowHighlight = false;
        ClearHighlightState();
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
        // Refresh completion state from the order reference
        isComplete = order != null && order.IsComplete;

        if (highlightCompleteImage != null)
            highlightCompleteImage.gameObject.SetActive(allowHighlight && isComplete);

        if (highlightHoverImage == null) return;

        if (!allowHighlight) {
            highlightHoverImage.gameObject.SetActive(false);
            return;
        }

        // If complete, suppress normal highlight (complete image handles it)
        if (isComplete) {
            highlightHoverImage.gameObject.SetActive(false);
            return;
        }

        // Show normal highlight when selected or hovered
        highlightHoverImage.gameObject.SetActive(isSelected || isHovered);
    }
}
