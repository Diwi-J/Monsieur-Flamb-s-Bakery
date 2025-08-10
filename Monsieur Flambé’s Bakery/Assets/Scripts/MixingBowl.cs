using UnityEngine;
using System.Collections.Generic;

public class MixingBowl : MonoBehaviour
{
    public List<string> requiredIngredients = new List<string> {
        "Egg", "Flour", "Sugar", "Water", "Butter", "Baking Powder", "Chocolate Chips", "Vanilla Essence", "Milk"
    };

    private List<string> addedIngredients = new List<string>(9);
    public Checklist checklist;

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null && !addedIngredients.Contains(ingredient.ingredientName))
        {
            string name = ingredient.ingredientName;

            addedIngredients.Add(name);

            Debug.Log("Added " + name + " to the bowl");

            if (checklist != null)
                checklist.MarkIngredientAsAdded(name);

            Destroy(other.gameObject);

            if (addedIngredients.Count == requiredIngredients.Count)
            {
                Debug.Log("All ingredients added! Ready to mix.");
            }
        }
    }
}
