using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] GameObject application;
    private RectTransform window;
    private Vector2 dragOffset;

    private void Awake()
    {
        window = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
}
