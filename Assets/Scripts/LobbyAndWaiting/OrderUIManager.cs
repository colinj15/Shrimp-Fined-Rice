using UnityEngine;

public class OrderUIManager : MonoBehaviour {
    public static OrderUIManager Instance;

    public GameObject ticketPrefab;
    public RectTransform[] ticketSlots;
    public RectTransform largeTicketSlot;   // parent on OrdersCanvas for enlarged ticket view

    private OrderTicketUI selectedTicket;
    private GameObject activeLargeTicket;
    private Canvas ordersCanvas;

    public OrderSystem.OrderData LastSelectedOrder { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnEnable() {
        OrderSystem.OnOrdersUpdated += RefreshUI;
        RefreshUI(); // catch up any orders that existed before this enabled
    }

    void OnDisable() {
        OrderSystem.OnOrdersUpdated -= RefreshUI;
    }

    void Update() {
        if (activeLargeTicket == null) return;

        // If player clicks anywhere not on the large ticket, remove it
        if (Input.GetMouseButtonDown(0) && !IsPointerOverLargeTicket()) {
            ClearLargeTicket();
        }
    }

    public void RefreshUI() {
        if (ticketSlots == null || ticketSlots.Length == 0) {
            Debug.LogWarning("[OrderUIManager] No ticket slots configured; cannot display orders.");
            return;
        }
        if (ticketPrefab == null) {
            Debug.LogError("[OrderUIManager] Ticket prefab is not assigned.");
            return;
        }

        // clear slot children
        foreach (var slot in ticketSlots) {
            if (slot == null) continue;

            foreach (Transform child in slot)
                Destroy(child.gameObject);
        }

        // spawn new tickets
        for (int i = 0; i < OrderSystem.ActiveOrders.Count; i++) {
            if (i >= ticketSlots.Length || ticketSlots[i] == null) {
                Debug.LogError($"[OrderUIManager] Missing ticket slot for order index {i}.");
                continue;
            }

            var ticketObj = Instantiate(ticketPrefab, ticketSlots[i]);
            // Ensure the instantiated UI stretches to the slot size (prevents oversized children)
            var ticketRect = ticketObj.transform as RectTransform;
            if (ticketRect != null) {
                ticketRect.anchorMin = Vector2.zero;
                ticketRect.anchorMax = Vector2.one;
                ticketRect.offsetMin = Vector2.zero;
                ticketRect.offsetMax = Vector2.zero;
                ticketRect.localScale = Vector3.one;
                ticketRect.localPosition = Vector3.zero;
            }

            var ui = ticketObj.GetComponent<OrderTicketUI>();
            if (ui == null) {
                Debug.LogError("[OrderUIManager] Ticket prefab is missing OrderTicketUI component.");
                continue;
            }

            ui.SetOrder(OrderSystem.ActiveOrders[i]);
        }
    }

    public void SelectTicket(OrderSystem.OrderData order, OrderTicketUI ticketUI) {
        if (selectedTicket != null)
            selectedTicket.SetHighlight(false);

        selectedTicket = ticketUI;
        LastSelectedOrder = order;

        // tell WaitingAreaManager which order is selected
        WaitingAreaManager.SelectedOrder = order;

        var largeTicketUI = ShowLargeTicket(order);

        // Disable highlighting on both the clicked ticket and its large copy
        selectedTicket?.ClearHighlightState();
        largeTicketUI?.ClearHighlightState();
    }

    public void ClearSelectedOrder() {
        LastSelectedOrder = null;
        selectedTicket = null;
    }

    private OrderTicketUI ShowLargeTicket(OrderSystem.OrderData order) {
        if (!EnsureLargeTicketSlot())
            return null;

        if (ticketPrefab == null) {
            Debug.LogError("[OrderUIManager] No ticket prefab assigned.");
            return null;
        }

        if (activeLargeTicket != null)
            Destroy(activeLargeTicket);

        activeLargeTicket = Instantiate(ticketPrefab, largeTicketSlot);
        // Ensure the large ticket renders on top by moving its parent and itself to the end of the hierarchy
        largeTicketSlot.SetAsLastSibling();
        activeLargeTicket.transform.SetAsLastSibling();

        // Stretch to fill the slot for consistent sizing
        if (activeLargeTicket.transform is RectTransform rect) {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localPosition = Vector3.zero;
        }

        var ui = activeLargeTicket.GetComponent<OrderTicketUI>();
        if (ui == null) {
            Debug.LogError("[OrderUIManager] Large ticket prefab is missing OrderTicketUI component.");
            return null;
        }

        ui.SetOrder(order);
        ui.DisableHighlighting(); // ensure large ticket never highlights (hover or select)
        return ui;
    }

    // Auto-resolve the large ticket slot if it was not assigned in inspector
    private bool EnsureLargeTicketSlot() {
        if (largeTicketSlot != null) return true;

        var ordersCanvas = GameObject.Find("OrdersCanvas");
        if (ordersCanvas != null) {
            this.ordersCanvas = ordersCanvas.GetComponent<Canvas>();
            var slot = ordersCanvas.transform.Find("LargeTicketSlot");
            if (slot != null)
                largeTicketSlot = slot as RectTransform;
        }
        else {
            // Attempt to find any canvas if OrdersCanvas is missing
            this.ordersCanvas = FindObjectOfType<Canvas>();
        }

        if (largeTicketSlot == null) {
            Debug.LogWarning("[OrderUIManager] Large ticket slot is not assigned and could not be auto-found (expects OrdersCanvas/LargeTicketSlot).");
            return false;
        }

        return true;
    }

    private bool IsPointerOverLargeTicket() {
        if (activeLargeTicket == null || largeTicketSlot == null) return false;

        var rect = activeLargeTicket.transform as RectTransform;
        if (rect == null) return false;

        // Use the canvas camera if available (for Screen Space - Camera), otherwise null works for Overlay
        var cam = ordersCanvas != null ? ordersCanvas.worldCamera : null;
        return RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, cam);
    }

    private void ClearLargeTicket() {
        if (activeLargeTicket != null) {
            Destroy(activeLargeTicket);
            activeLargeTicket = null;
        }
    }
}
