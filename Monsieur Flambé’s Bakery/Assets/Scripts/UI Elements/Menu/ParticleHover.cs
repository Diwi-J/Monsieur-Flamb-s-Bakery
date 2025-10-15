using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverParticles : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ParticleSystem hoverParticles;

    public void OnPointerEnter(PointerEventData eventData) => hoverParticles?.Play();
    public void OnPointerExit(PointerEventData eventData) => hoverParticles?.Stop();

    public void OnSelect(BaseEventData eventData) => hoverParticles?.Play();
    public void OnDeselect(BaseEventData eventData) => hoverParticles?.Stop();
}
