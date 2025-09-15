using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : Interactable
{
    [Header("Ingredients (unique names)")]
    [SerializeField]
    private List<string> requiredIngredients = new List<string>
    {
        "Egg", "Flour", "Sugar", "Water", "Butter", "Baking Powder", "Vanilla Essence", "Milk"
    };

    // Tracks unique ingredients added to the bowl.
    private readonly HashSet<string> addedUnique = new HashSet<string>();

    [Header("Visuals")]
    [SerializeField] private GameObject rawIngredientsVisual;
    [SerializeField] private GameObject mixtureVisual;

    [Header("Result")]
    [SerializeField] private GameObject mixturePrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.75f, 0);

    [Header("Recipe Book")]
    public RecipeBookUI recipeBookUI;

    private bool isMixed = false;

    private void Start()
    {
        // Hide visuals at start
        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(false);
        if (mixtureVisual) mixtureVisual.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item == null) return;

        string ingName = other.gameObject.name.Trim();

        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(true);

        if (!addedUnique.Contains(ingName))
        {
            addedUnique.Add(ingName);

            // Auto-tick in recipe book
            if (recipeBookUI != null)
                AddIngredient(ingName);

            Debug.Log($"[MixingBowl] Added ingredient: {ingName} ({addedUnique.Count}/{requiredIngredients.Count})");
        }
        else
        {
            Debug.Log($"[MixingBowl] Duplicate ingredient ignored: {ingName}");
        }

        // Remove the ingredient object from the scene
        Destroy(other.gameObject);

        // Complete mixture instantly if all ingredients added
        if (!isMixed && addedUnique.Count >= requiredIngredients.Count)
            CompleteInstantly();
    }

    public override void Interact()
    {
        if (!isMixed && addedUnique.Count >= requiredIngredients.Count)
            CompleteInstantly();
    }

    private void CompleteInstantly()
    {
        isMixed = true;

        // Hide visuals
        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(false);
        if (mixtureVisual) mixtureVisual.SetActive(false);

        if (mixturePrefab == null)
        {
            Debug.LogError("[MixingBowl] No mixturePrefab assigned!");
            return;
        }

        // Spawn finished mixture
        GameObject finishedMixture = Instantiate(mixturePrefab, transform.position + spawnOffset, Quaternion.identity);

        // Ensure collider
        if (finishedMixture.GetComponent<Collider>() == null)
            finishedMixture.AddComponent<BoxCollider>();

        // Ensure Rigidbody
        Rigidbody rb = finishedMixture.GetComponent<Rigidbody>();
        if (rb == null)
            rb = finishedMixture.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = true;

        // Ensure PickupItem
        PickupItem pickupItem = finishedMixture.GetComponent<PickupItem>();
        if (pickupItem == null)
            pickupItem = finishedMixture.AddComponent<PickupItem>();

        // Auto-pickup
        PlayerInteractable player = FindObjectOfType<PlayerInteractable>();
        if (player != null && player.hand != null)
        {
            if (player.heldItem != null)
                player.DropItem();

            pickupItem.PickUp(player.hand);
            player.heldItem = pickupItem;
            Debug.Log("[MixingBowl] Mixture auto-picked up into player's hand.");
        }

        // Destroy bowl after mixing
        Destroy(gameObject);

        // Advance game stage safely
        try { GameManager.Instance.AdvanceStage(CakeStage.MixtureReady); } catch { }
    }

    public bool IsMixed() => isMixed;

    public void AddIngredient(string ingredientName)
    {
        Debug.Log($"Added {ingredientName} to the bowl");

        // Auto-tick in the recipe book
        recipeBookUI?.AutoTickIngredient(ingredientName);

        // Check if recipe is complete
        if (recipeBookUI != null && recipeBookUI.IsRecipeComplete())
            Debug.Log("Recipe complete! Ready to bake!");
    }
}
