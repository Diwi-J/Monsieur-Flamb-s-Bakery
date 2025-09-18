using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CakeStage CurrentStage { get; private set; } = CakeStage.AddingIngredients;

    private void Awake()
    {
        //Ensures that only one instance of GameManager exists.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); //Prevents duplicates.
    }

    public void AdvanceStage(CakeStage newStage)
    {
        //Checks if the new stage is valid.
        CurrentStage = newStage;
        Debug.Log($"GameManager: Stage changed to {newStage}");
    }
}


