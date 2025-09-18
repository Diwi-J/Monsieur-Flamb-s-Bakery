using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeBookUI : MonoBehaviour
{
    [System.Serializable]
    public class IngredientEntry
    {
        public string ingredientName;
        public Toggle toggle;
    }

    public IngredientEntry[] ingredients;

    private HashSet<string> collectedIngredients = new HashSet<string>();

    void Start()
    {
        // Initialize toggles and listeners
        foreach (var entry in ingredients)
        {
            entry.toggle.isOn = false;
            entry.toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(entry, isOn));
        }
    }

    private void OnToggleChanged(IngredientEntry entry, bool isOn)
    {
        if (isOn)
            collectedIngredients.Add(entry.ingredientName);
        else
            collectedIngredients.Remove(entry.ingredientName);

        Debug.Log($"Ingredient {entry.ingredientName} is now {(isOn ? "checked" : "unchecked")}");
    }

    // Called by the game logic when an ingredient is added physically
    public void AutoTickIngredient(string ingredientName)
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (ingredients[i].ingredientName == ingredientName)
            {
                ingredients[i].toggle.isOn = true;
                break;
            }
        }
    }

    public bool IsRecipeComplete()
    {
        return collectedIngredients.Count == ingredients.Length;
    }
}