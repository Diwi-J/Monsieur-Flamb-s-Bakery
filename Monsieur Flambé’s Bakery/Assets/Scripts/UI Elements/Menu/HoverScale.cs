using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScaleUniversal : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    public float hoverScale = 1.2f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
