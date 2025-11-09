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
        //Store the original scale.
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Enlarge on hover or selection.
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Revert to original scale.
        transform.localScale = originalScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Enlarge on hover or selection.
        transform.localScale = originalScale * hoverScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Revert to original scale.
        transform.localScale = originalScale;
    }
}
