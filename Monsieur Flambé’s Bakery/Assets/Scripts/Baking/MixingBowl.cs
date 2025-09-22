using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : Interactable
{
    [Header("Ingredients (unique names)")]
    [SerializeField]
    private List<string> requiredIngredients = new List<string>
    {
        "Egg", "Flour", "Sugar", "Water", "Butter", "Baking Powder", "Vanilla Essence"
    };

    private readonly HashSet<string> addedUnique = new HashSet<string>();

    [Header("Visuals")]
    [SerializeField] private GameObject rawIngredientsVisual;
    [SerializeField] private GameObject mixtureVisual;

    [Header("Recipe Book")]
    public RecipeBookUI recipeBookUI;

    private bool isMixed = false;
    private PickupItem pickupItem;

    private void Start()
    {
        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(false);
        if (mixtureVisual) mixtureVisual.SetActive(false);

        pickupItem = GetComponent<PickupItem>();
        if (pickupItem != null)
            pickupItem.enabled = false; //Cannot pick up until mixed
    }

    //When an ingredient enters the bowl
    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item == null || isMixed) return;

        string ingName = other.gameObject.name.Trim();

        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(true);

        if (!addedUnique.Contains(ingName))
        {
            addedUnique.Add(ingName);
            recipeBookUI?.AutoTickIngredient(ingName);

        }

        Destroy(other.gameObject);

        if (addedUnique.Count >= requiredIngredients.Count)
            CompleteMixture();
    }


    //Player interaction to finalize mixing if all ingredients are present
    public override void Interact()
    {
        if (!isMixed && addedUnique.Count >= requiredIngredients.Count)
            CompleteMixture();
    }


    //Called when all ingredients are added
    private void CompleteMixture()
    {
        isMixed = true;

        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(false);
        if (mixtureVisual) mixtureVisual.SetActive(true);

        if (pickupItem != null)
            pickupItem.enabled = true; //Now the player can pick it up

        if (recipeBookUI != null && recipeBookUI.IsRecipeComplete())
            Debug.Log("Recipe book shows all ingredients completed!");
    }

    public bool IsMixed() => isMixed;
}
