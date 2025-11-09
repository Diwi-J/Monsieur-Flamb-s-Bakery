using UnityEngine;
using TMPro;

public class FloatingPrompt : MonoBehaviour
{
    public static FloatingPrompt Instance; 

    [Header("UI Text")]
    public TMP_Text promptText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (promptText == null)
        {
            Debug.LogWarning("FloatingPrompt- No TextMeshProUGUI assigned!");
        }
        else
        {
            promptText.text = "";
        }
    }

    public void Show(string message)
    {
        //Show the prompt message.
        if (promptText != null)
            promptText.text = message;
    }

    public void Hide()
    {
        //Hide the prompt message.
        if (promptText != null)
            promptText.text = "";
    }
}
