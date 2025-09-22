using UnityEngine;

public class NPC : Interactable
{
    [Header("Dialogue")]
    public Dialogue blockedDialogue;  // if cake not placed
    public Dialogue mainDialogue;     // normal dialogue once cake is placed

    [Header("Cake Target Zone")]
    public CakeTargetZone cakeZone;   // assign in Inspector

    private bool unlocked = false;

    public void UnlockDialogue()
    {
        unlocked = true;
    }

    public override void Interact()
    {
        if (cakeZone != null && !cakeZone.IsCakePlaced())
        {
            // Cake not placed, show blocked dialogue
            DialogueManager.Instance.StartDialogue(blockedDialogue);
        }
        else
        {
            // Cake placed or unlocked manually
            DialogueManager.Instance.StartDialogue(mainDialogue);
        }
    }
}
