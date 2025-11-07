using UnityEngine;
using System.Linq;

public class NPC : Interactable
{
    [Header("Dialogue")]
    public Dialogue blockedDialogue;
    public Dialogue mainDialogue;

    [Header("Cake Target Zone")]
    public CakeTargetZone cakeZone;

    [Header("Celebration")]
    public bool triggersCelebration = false;

    [Header("Door Unlock (optional)")]
    public HingeDoor linkedDoor;

    private bool unlocked = false;

    public bool HasBeenSpokenTo => unlocked;

    public void UnlockDialogue()
    {
        unlocked = true;
    }

    public override void Interact()
    {
        unlocked = true;

        bool cakeInZone = false;
        if (cakeZone != null)
        {
            // Dynamically check for any cake in zone
            cakeInZone = cakeZone.IsCakePlaced();
        }

        if (!cakeInZone)
        {
            DialogueManager.Instance.StartDialogue(blockedDialogue, false);
        }
        else
        {
            if (triggersCelebration)
                DialogueManager.Instance.StartDialogue(mainDialogue, true, transform);
            else
                DialogueManager.Instance.StartDialogue(mainDialogue, false);
        }

        if (linkedDoor != null)
        {
            linkedDoor.TryOpenDoor();
            Debug.Log("Door unlocked!");
        }
    }
}


