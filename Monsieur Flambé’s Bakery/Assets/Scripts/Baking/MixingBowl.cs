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

    //This tracks unique ingredients added to the bowl.
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
        //Ensures visuals are hidden at start.
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

        //Removes the dropped ingredient from the scene after being added to the mixing bowl.
        Destroy(other.gameObject);

        //Completes mixture instantly when all ingredients are added.
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

        //Hides visuals.
        if (rawIngredientsVisual) rawIngredientsVisual.SetActive(false);
        if (mixtureVisual) mixtureVisual.SetActive(false);

        if (mixturePrefab == null)
        {
            Debug.LogError("[MixingBowl] No mixturePrefab assigned!");
        }
        else
        {
            //Spawns mixture prefab.
            GameObject finishedMixture = Instantiate(mixturePrefab, transform.position + spawnOffset, Quaternion.identity);
            finishedMixture.SetActive(true);

            //Ensures the Collider component.
            if (finishedMixture.GetComponent<Collider>() == null)
                finishedMixture.AddComponent<BoxCollider>();

            //Ensures the Rigidbody component.
            Rigidbody rb = finishedMixture.GetComponent<Rigidbody>();
            if (rb == null)
                rb = finishedMixture.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = true;

            //Ensures PickupItem.
            PickupItem pickupItem = finishedMixture.GetComponent<PickupItem>();
            if (pickupItem == null)
                pickupItem = finishedMixture.AddComponent<PickupItem>();

            //Auto-pickup mixture prefab.
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

        //Destroys the bowl object after mixing is complete.
        Destroy(gameObject);

        //Advance game stage safely.
        try { GameManager.Instance.AdvanceStage(CakeStage.MixtureReady); } catch { }
    }

    public bool IsMixed() => isMixed;
}
