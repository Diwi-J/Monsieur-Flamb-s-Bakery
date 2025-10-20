using UnityEngine;

public class NPC : Interactable
{
    [Header("Dialogue")]
    public Dialogue blockedDialogue;  // if cake not placed
    public Dialogue mainDialogue;     // normal dialogue once cake is placed

    [Header("Cake Target Zone")]
    public CakeTargetZone cakeZone;   // assign in Inspector

    [Header("Celebration")]
    public bool triggersCelebration = false;  // Only this NPC can trigger confetti

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
            DialogueManager.Instance.StartDialogue(blockedDialogue, false);
        }
        else
        {
            // Cake placed or unlocked manually
            // Only trigger celebration if this NPC is flagged
            if (triggersCelebration)
            {
                DialogueManager.Instance.StartDialogue(mainDialogue, true, transform);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(mainDialogue, false);
            }
        }
    }
}

