using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : Interactable
{
    [SerializeField]
    private List<string> requiredIngredients = new List<string>
    {
        "Egg", "Flour", "Sugar", "Water", "Butter", "Baking Powder", "Vanilla Essence"
    };

    private List<string> addedIngredients = new List<string>();

    [SerializeField] private GameObject rawIngredientsVisual;
    [SerializeField] private GameObject mixtureVisual;
    [SerializeField] private float mixingDuration = 3f;
    [SerializeField] private GameObject mixturePrefab;
    [SerializeField] private Transform hand; 

    private bool isMixed = false;
    public Checklist checklist;
    private bool playerInRange = false;


    private void Start()
    {
        rawIngredientsVisual?.SetActive(false);
        mixtureVisual?.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null)
        {
            // Use the name of the item as ingredient
            AddIngredient(item.gameObject.name);

            // Optional: destroy or hide the ingredient
            Destroy(item.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }


    public void AddIngredient(string ingredientName)
    {
        if (isMixed) return;

        if (!addedIngredients.Contains(ingredientName))
        {
            addedIngredients.Add(ingredientName);
            rawIngredientsVisual?.SetActive(true);

            checklist?.MarkIngredientAsAdded(ingredientName);

            if (addedIngredients.Count >= requiredIngredients.Count)
            {
                GameManager.Instance.AdvanceStage(CakeStage.Mixing);
            }
        }
    }

    public void StartMixing()
    {
        if (isMixed) return;
        if (addedIngredients.Count < requiredIngredients.Count) return;

        StartCoroutine(MixCoroutine());
    }

    public override void Interact()
    {
        // Either start the mixing coroutine
        // or instantly finish mixture
        IsMixed();
        Debug.Log("Bowl interacted with!");
    }




    private IEnumerator MixCoroutine()
    {
        // Show raw ingredients visual while mixing
        rawIngredientsVisual?.SetActive(true);
        mixtureVisual?.SetActive(false);

        yield return new WaitForSeconds(mixingDuration);

        // Mixing finished
        isMixed = true;

        // Hide the original bowl visuals
        rawIngredientsVisual?.SetActive(false);
        mixtureVisual?.SetActive(false);

        // Spawn the finished mixture prefab at the bowl’s position
        if (mixturePrefab != null)
        {
            GameObject finishedMixture = Instantiate(mixturePrefab, transform.position, Quaternion.identity);

            // Make the player pick it up automatically
            PlayerInteractable playerInteractable = FindObjectOfType<PlayerInteractable>();
            if (playerInteractable != null && finishedMixture.TryGetComponent<PickupItem>(out PickupItem item))
            {
                item.PickUp(playerInteractable.hand);
                playerInteractable.heldItem = item;
            }
        }

        // Destroy or deactivate the original bowl
        gameObject.SetActive(false);

        GameManager.Instance.AdvanceStage(CakeStage.MixtureReady);
    }


    public bool IsMixed() => isMixed;

    public void ResetBowl()
    {
        isMixed = false;
        addedIngredients.Clear();
        rawIngredientsVisual?.SetActive(false);
        mixtureVisual?.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
