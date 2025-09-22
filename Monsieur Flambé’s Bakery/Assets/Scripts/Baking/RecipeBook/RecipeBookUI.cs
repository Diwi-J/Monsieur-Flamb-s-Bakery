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

    void Awake()
    {
        //Initialize toggles safely
        foreach (var entry in ingredients)
        {
            if (entry.toggle != null)
            {
                entry.toggle.isOn = false;
                entry.toggle.interactable = true;
                entry.toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(entry, isOn));
            }
            else
            {
                Debug.LogWarning($"[RecipeBookUI] Toggle not assigned for ingredient: {entry.ingredientName}");
            }
        }

        collectedIngredients.Clear();
    }

    //Handles toggle changes
    private void OnToggleChanged(IngredientEntry entry, bool isOn)
    {
        if (isOn)
            collectedIngredients.Add(entry.ingredientName.Trim());
        else
            collectedIngredients.Remove(entry.ingredientName.Trim());
    }

    public void AutoTickIngredient(string ingredientName)
    {
        string trimmedName = ingredientName.Trim();

        bool found = false;
        foreach (var entry in ingredients)
        {
            if (entry.ingredientName.Trim() == trimmedName)
            {
                found = true;
                if (!entry.toggle.isOn)
                {
                    entry.toggle.isOn = true; // Will trigger OnToggleChanged
                }
                //Ensure the ingredient is in the collected set
                collectedIngredients.Add(trimmedName);
                break;
            }
        }

        if (!found)
            Debug.LogWarning($"[RecipeBookUI] Ingredient not found: {trimmedName}");
    }

    public bool IsRecipeComplete()
    {
        return collectedIngredients.Count == ingredients.Length;
    }

    /*Reset the book
    public void ResetBook()
    {
        foreach (var entry in ingredients)
        {
            if (entry.toggle != null)
                entry.toggle.isOn = false;
        }
        collectedIngredients.Clear();
    }*/
}

