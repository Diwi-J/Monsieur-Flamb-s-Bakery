using UnityEngine;
using TMPro;

public class FloatingPrompt : MonoBehaviour
{
    public static FloatingPrompt Instance; // Singleton

    [Header("UI Text")]
    public TMP_Text promptText; // Assign your TextMeshProUGUI

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
            Debug.LogWarning("[FloatingPrompt] No TextMeshProUGUI assigned!");
        }
        else
        {
            promptText.text = "";
        }
    }

    public void Show(string message)
    {
        if (promptText != null)
            promptText.text = message;
    }

    public void Hide()
    {
        if (promptText != null)
            promptText.text = "";
    }
}
