using UnityEngine;
using UnityEngine.EventSystems;

public class WindowController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
     [SerializeField] GameObject application;
    private RectTransform window;
    private Vector2 dragOffset;

    private void Awake()
    {
        // The window is the parent of the title bar
        window = transform.parent.GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Bring this window to the front when you start dragging it
        window.SetAsLastSibling();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            window,
            eventData.position,
            eventData.pressEventCamera,
            out dragOffset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            window.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        window.anchoredPosition = pos - dragOffset;
    }

    public void CloseApplication()
    {
        application.SetActive(false);
    }

    public void OpenApplication(GameObject app)
    {
        app.SetActive(true);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
