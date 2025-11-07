using UnityEngine;

public class CakeTargetZone : MonoBehaviour
{
    [Header("Required Tag")]
    public string cakeTag = "Cake"; // make sure your cake prefab has this tag!

    [Header("Linked NPC")]
    public NPC npcToUnlock;

    [Header("Timer Reference (Optional)")]
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

            // Unlock second NPC
            if (npcToUnlock != null)
                npcToUnlock.UnlockDialogue();

            // Stop the timer (success)
            if (gameTimer != null)
                gameTimer.StopTimerForObjective();
        }
    }

}
