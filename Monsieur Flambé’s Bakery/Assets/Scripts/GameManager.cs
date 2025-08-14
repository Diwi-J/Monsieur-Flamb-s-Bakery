using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CakeStage CurrentStage { get; private set; } = CakeStage.AddingIngredients;

    private void Awake()
    {
        //Ensure that only one instance of GameManager exists
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); //Prevent duplicates
    }

    public void AdvanceStage(CakeStage newStage)
    {
        //Check if the new stage is valid
        CurrentStage = newStage;
        Debug.Log($"GameManager: Stage changed to {newStage}");
    }
}

