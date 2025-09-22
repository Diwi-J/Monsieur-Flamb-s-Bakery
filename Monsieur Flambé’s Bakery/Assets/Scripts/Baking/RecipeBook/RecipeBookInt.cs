using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeBookToggle : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject recipeBookCanvas;
    private bool isOpen = false;

    private void Start()
    {
        //Start a coroutine to wait one frame to ensure toggles are fully initialized
        StartCoroutine(ResetTogglesNextFrame());
    }

    private IEnumerator ResetTogglesNextFrame()
    {
        yield return null; //wait one frame

        Toggle[] toggles = GetComponentsInChildren<Toggle>(true);
        foreach (Toggle t in toggles)
        {
            t.isOn = false; //force unchecked toggle
        }
    }

    public void ToggleRecipeBook()
    {
        if (recipeBookCanvas == null) return;

        isOpen = !isOpen;                  
        recipeBookCanvas.SetActive(isOpen);
    }
    public bool IsOpen() => isOpen;
}
