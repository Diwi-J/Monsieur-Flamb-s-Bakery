using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : MonoBehaviour
{
    [SerializeField]
    private List<string> requiredIngredients = new List<string>
    {
        "Egg", "Flour", "Sugar", "Water", "Butter", "Baking Powder", "Chocolate Chips", "Vanilla Essence"
    };

    private List<string> addedIngredients = new List<string>();

    [SerializeField] private GameObject rawIngredientsVisual;
    [SerializeField] private GameObject mixtureVisual;

    [SerializeField] private float mixingDuration = 3f;

    private bool isMixed = false;
    public Checklist checklist;

    private void Start()
    {
        rawIngredientsVisual?.SetActive(false);
        mixtureVisual?.SetActive(false);
    }

    public void AddIngredient(string ingredientName)
    {
        if (isMixed) return;

        if (!addedIngredients.Contains(ingredientName))
        {
            addedIngredients.Add(ingredientName);
            rawIngredientsVisual?.SetActive(true);

            Debug.Log($"Added {ingredientName} ({addedIngredients.Count}/{requiredIngredients.Count})");

            checklist?.MarkIngredientAsAdded(ingredientName);

            if (addedIngredients.Count >= requiredIngredients.Count)
            {
                Debug.Log("All ingredients added! Ready to mix.");
                GameManager.Instance.AdvanceStage(CakeStage.Mixing);
            }
        }
    }

    public void StartMixing()
    {
        if (isMixed)
        {
            Debug.Log("Already mixed.");
            return;
        }

        if (addedIngredients.Count < requiredIngredients.Count)
        {
            Debug.LogWarning("Not all ingredients added yet.");
            return;
        }

        StartCoroutine(MixCoroutine());
    }

    private IEnumerator MixCoroutine()
    {
        Debug.Log("Mixing started...");
        rawIngredientsVisual?.SetActive(true);
        mixtureVisual?.SetActive(false);

        yield return new WaitForSeconds(mixingDuration);

        isMixed = true;
        Debug.Log("Mixing finished.");

        rawIngredientsVisual?.SetActive(false);
        mixtureVisual?.SetActive(true);

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
}
