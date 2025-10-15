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
    [SerializeField] private Transform fillVisual; //The liquid or ingredient pile visual
    [SerializeField] private float fillMaxHeight = 0.2f; // how high it fills
    [SerializeField] private float fillSmoothSpeed = 3f; // how smooth the fill transitions

    [Header("Recipe Book")]
    public RecipeBookUI recipeBookUI;

    private bool isMixed = false;
    private PickupItem pickupItem;
    private float targetFillLevel = 0f;
    private float currentFillLevel = 0f;

    private void Start()
    {
        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(false);
        if (mixtureVisual) mixtureVisual.SetActive(false);

        pickupItem = GetComponent<PickupItem>();
        if (pickupItem != null)
            pickupItem.enabled = false; //Cannot pick up until mixed

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

            // Update fill target based on progress
            targetFillLevel = (float)addedUnique.Count / requiredIngredients.Count;
        }

        Destroy(other.gameObject);

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

        // Keep the fill visible
        if (fillVisual != null)
        {
            // Optional: change color to indicate "mixed"
            Renderer rend = fillVisual.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = new Color(0.9f, 0.75f, 0.5f); // creamy brownish batter
            }
        }

        // Enable pickup now that it’s fully mixed
        if (pickupItem != null)
            pickupItem.enabled = true;
    }


    public bool IsMixed() => isMixed;
}
