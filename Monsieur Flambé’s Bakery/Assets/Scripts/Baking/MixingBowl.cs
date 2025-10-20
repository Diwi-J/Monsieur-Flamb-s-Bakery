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
    [SerializeField] private Transform fillVisual; // The sphere or liquid visual
    [SerializeField] private float fillMaxHeight = 0.2f; // max Y scale for full mixture
    [SerializeField] private float fillSmoothSpeed = 3f; // speed of filling animation

    [Header("Recipe Book")]
    public RecipeBookUI recipeBookUI;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem addIngredientParticles;

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
            pickupItem.canPickUp = false; // ❌ Prevent pickup until all ingredients are added

        // Start fill at 0
        if (fillVisual != null)
        {
            Vector3 scale = fillVisual.localScale;
            fillVisual.localScale = new Vector3(scale.x, 0f, scale.z);
        }
    }

    private void Update()
    {
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

        string ingName = other.gameObject.name.Replace("(Clone)", "").Trim();

        if (!addedUnique.Contains(ingName))
        {
            addedUnique.Add(ingName);
            recipeBookUI?.AutoTickIngredient(ingName);

            targetFillLevel = (float)addedUnique.Count / requiredIngredients.Count;

            // Spawn ingredient puff particles at the bowl
            if (addIngredientParticles != null)
            {
                Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
                ParticleSystem ps = Instantiate(addIngredientParticles, spawnPos, Quaternion.identity);
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
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

        if (fillVisual != null)
        {
            Renderer rend = fillVisual.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = new Color(0.9f, 0.75f, 0.5f); // creamy brown
        }

        // ✅ Only change: enable pickup now that all ingredients are added
        if (pickupItem != null)
            pickupItem.canPickUp = true;

        if (recipeBookUI != null && recipeBookUI.IsRecipeComplete())
            Debug.Log("Recipe book shows all ingredients completed!");
    }

    public bool IsMixed() => isMixed;

    public void AddIngredient()
    {
        ingredientsAdded++;

        if (ingredientsAdded >= totalIngredientsRequired)
        {
            // Enable pickup
            pickupItem.canPickUp = true;
        }
        else
        {
            // Keep pickup disabled until all added
            pickupItem.canPickUp = false;
        }
    }
}
