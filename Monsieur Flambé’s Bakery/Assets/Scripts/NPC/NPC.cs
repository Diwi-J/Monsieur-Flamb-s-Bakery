using UnityEngine;

public class NPC : Interactable
{
    [Header("Dialogue")]
    public Dialogue blockedDialogue;  // Dialogue if cake not placed
    public Dialogue mainDialogue;     // Normal dialogue once cake is placed

    [Header("Cake Target Zone")]
    public CakeTargetZone cakeZone;   // Assign in Inspector

    [Header("Celebration")]
    public bool triggersCelebration = false;  // Only this NPC triggers confetti

    [Header("Door Unlock (optional)")]
    public HingeDoor linkedDoor; // Assign the door this NPC unlocks (optional)

    private bool unlocked = false;

    // Public property to let other scripts check if NPC has been spoken to
    public bool HasBeenSpokenTo => unlocked;

    public void UnlockDialogue()
    {
        unlocked = true;
    }

    public override void Interact()
    {
        // Mark NPC as spoken to
        unlocked = true;

        // Start appropriate dialogue
        if (cakeZone != null && !cakeZone.IsCakePlaced())
        {
            DialogueManager.Instance.StartDialogue(blockedDialogue, false);
        }
        else
        {
            if (triggersCelebration)
            {
                DialogueManager.Instance.StartDialogue(mainDialogue, true, transform);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(mainDialogue, false);
            }
        }

        // Unlock linked door (optional)
        if (linkedDoor != null)
        {
            linkedDoor.TryOpenDoor();
            Debug.Log("Door unlocked!");
        }
    }
}
