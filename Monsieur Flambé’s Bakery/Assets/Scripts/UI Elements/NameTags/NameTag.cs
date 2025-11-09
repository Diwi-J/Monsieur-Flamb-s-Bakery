using TMPro;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; //Start hidden.
    }

    public void SetName(string name)
    {
        //Set the text of the name tag.
        if (textUI != null)
            textUI.text = name;
    }

    public void Show(float fadeSpeed = 5f)
    {
        //Fade in the name tag.
        if (canvasGroup != null)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * fadeSpeed);
    }

    public void Hide(float fadeSpeed = 5f)
    {
        //Fade out the name tag.
        if (canvasGroup != null)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime * fadeSpeed);
    }
}
