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

    [Header("Result")]
    [SerializeField] private GameObject mixturePrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.75f, 0);

    [Header("Other")]
    public Checklist checklist;
    private bool isMixed = false;

    private void Start()
    {
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
            if (checklist) checklist.MarkIngredientAsAdded(ingName);
            Debug.Log($"[MixingBowl] Added ingredient: {ingName} ({addedUnique.Count}/{requiredIngredients.Count})");
        }
        else
        {
            Debug.Log($"[MixingBowl] Duplicate ingredient ignored: {ingName}");
        }

        // Remove the dropped ingredient
        Destroy(other.gameObject);

        // Complete instantly when all ingredients are added
        if (!isMixed && addedUnique.Count >= requiredIngredients.Count)
        {
            CompleteInstantly();
        }
    }

    public override void Interact()
    {
        if (!isMixed && addedUnique.Count >= requiredIngredients.Count)
        {
            CompleteInstantly();
        }
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
        }
        else
        {
            // Spawn mixture prefab
            GameObject finishedMixture = Instantiate(mixturePrefab, transform.position + spawnOffset, Quaternion.identity);
            finishedMixture.SetActive(true);

            // Ensure Collider
            if (finishedMixture.GetComponent<Collider>() == null)
                finishedMixture.AddComponent<BoxCollider>();

            // Ensure Rigidbody
            Rigidbody rb = finishedMixture.GetComponent<Rigidbody>();
            if (rb == null)
                rb = finishedMixture.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = true; // important for pickup

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
            else
            {
                Debug.LogWarning("[MixingBowl] Player or hand not found. Mixture stays in scene.");
            }
        }

        // Destroy the bowl
        Destroy(gameObject);

        // Advance game stage safely
        try { GameManager.Instance.AdvanceStage(CakeStage.MixtureReady); } catch { }
    }

    public bool IsMixed() => isMixed;
}
