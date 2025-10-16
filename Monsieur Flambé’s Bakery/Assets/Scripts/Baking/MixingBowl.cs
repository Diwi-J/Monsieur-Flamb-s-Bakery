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
    [SerializeField] private Transform fillVisual;
    [SerializeField] private float fillMaxHeight = 0.2f;
    [SerializeField] private float fillSmoothSpeed = 3f;

    [Header("Recipe Book")]
    public RecipeBookUI recipeBookUI;

    private bool isMixed = false;
    private PickupItem pickupItem;
    private float targetFillLevel = 0f;
    private float currentFillLevel = 0f;

    public int ingredientsAdded = 0;
    public int totalIngredientsRequired = 7;

    private void Start()
    {
        pickupItem = GetComponent<PickupItem>();
        if (pickupItem != null)
            pickupItem.canPickUp = false; // prevent early pickup

        // Start fill at 0
        if (fillVisual != null)
        {
            Vector3 scale = fillVisual.localScale;
            fillVisual.localScale = new Vector3(scale.x, 0f, scale.z);
        }
    }

    private void Update()
    {
        // Smoothly animate the filling
        if (fillVisual != null && !isMixed)
        {
            currentFillLevel = Mathf.Lerp(currentFillLevel, targetFillLevel, Time.deltaTime * fillSmoothSpeed);
            Vector3 scale = fillVisual.localScale;
            fillVisual.localScale = new Vector3(scale.x, currentFillLevel * fillMaxHeight, scale.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMixed) return;

        PickupItem item = other.GetComponent<PickupItem>();
        if (item == null) return;

        string ingName = other.gameObject.name.Trim();

        if (!addedUnique.Contains(ingName))
        {
            addedUnique.Add(ingName);
            recipeBookUI?.AutoTickIngredient(ingName);

            targetFillLevel = (float)addedUnique.Count / requiredIngredients.Count;
        }

        Destroy(other.gameObject);

        // Check if all ingredients are added
        if (addedUnique.Count >= requiredIngredients.Count)
            CompleteMixture();
    }

    public override void Interact()
    {
        if (!isMixed && addedUnique.Count >= requiredIngredients.Count)
            CompleteMixture();
    }

    private void CompleteMixture()
    {
        isMixed = true;

        // Fill visual recolor
        if (fillVisual != null)
        {
            Renderer rend = fillVisual.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = new Color(0.9f, 0.75f, 0.5f); // creamy brown
        }

        // Enable pickup now that mixture is complete
        if (pickupItem != null)
        {
            pickupItem.canPickUp = true;  // ✅ allow pickup
        }

        if (recipeBookUI != null && recipeBookUI.IsRecipeComplete())
            Debug.Log("Recipe book shows all ingredients completed!");
    }

    // Optional: manual ingredient counter (can be used by UI buttons)
    public void AddIngredient()
    {
        ingredientsAdded++;

        if (ingredientsAdded >= totalIngredientsRequired && pickupItem != null)
            pickupItem.canPickUp = true;
        else if (pickupItem != null)
            pickupItem.canPickUp = false;
    }

    public bool IsMixed() => isMixed;
}
