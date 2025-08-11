using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CakeStage CurrentStage { get; private set; } = CakeStage.AddingIngredients;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // prevent duplicates
    }

    public void AdvanceStage(CakeStage newStage)
    {
        CurrentStage = newStage;
        Debug.Log($"GameManager: Stage changed to {newStage}");
    }
}

