using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverParticles : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ParticleSystem hoverParticles;

    public void OnPointerEnter(PointerEventData eventData) => hoverParticles?.Play(); //Play particles.
    public void OnPointerExit(PointerEventData eventData) => hoverParticles?.Stop(); //Stop particles.

    public void OnSelect(BaseEventData eventData) => hoverParticles?.Play(); //Play particles.
    public void OnDeselect(BaseEventData eventData) => hoverParticles?.Stop(); //Stop particles.
}
