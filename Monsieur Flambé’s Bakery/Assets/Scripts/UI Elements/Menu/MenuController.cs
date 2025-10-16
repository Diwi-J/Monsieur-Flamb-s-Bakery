using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [Header("UI Navigation")]
    public GameObject firstSelectedButton;  // Drag your first button here

    void OnEnable()
    {
        StartCoroutine(SelectFirstButton());
    }

    private IEnumerator SelectFirstButton()
    {
        // Wait one frame so Unity finishes enabling UI elements
        yield return null;

        // Clear selection (fixes disappearing buttons)
        EventSystem.current.SetSelectedGameObject(null);

        // Set your first button as selected
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }
}
