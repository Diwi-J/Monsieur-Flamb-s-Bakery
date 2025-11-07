using UnityEngine;

public class CakeTargetZone : MonoBehaviour
{
    [Header("Required Tag")]
    public string cakeTag = "Cake";

    [Header("Linked NPC")]
    public NPC npcToUnlock;

    [Header("Timer")]
    public GameTimer gameTimer;

    private bool cakePlaced = false;
    public bool IsCakePlaced() => cakePlaced;

    private void OnTriggerEnter(Collider other)
    {
        if (cakePlaced) return;

        if (other.CompareTag(cakeTag))
        {
            cakePlaced = true;
            Debug.Log("[CakeTargetZone] Cake placed!");

            // Unlock NPC dialogue
            if (npcToUnlock != null)
                npcToUnlock.UnlockDialogue();

            // Stop timer (success)
            if (gameTimer != null)
                gameTimer.StopTimerForObjective();
        }
    }
}

