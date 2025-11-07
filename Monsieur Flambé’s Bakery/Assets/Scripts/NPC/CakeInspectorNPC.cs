using UnityEngine;

public class CakeInspectorNPC : Interactable
{
    [Header("References")]
    public CakeTargetZone cakeTargetZone; // Assign your CakeTargetZone
    public NPC linkedNPC;                 // Assign the NPC that should talk

    public override void Interact()
    {
        if (cakeTargetZone != null && !cakeTargetZone.IsCakePlaced())
        {
            Debug.Log("I can't talk to you until the cake is placed!");
            // Optional: show UI hint to player instead of just Debug.Log
            return;
        }

        // Forward interaction to the linked NPC for normal dialogue/celebration
        if (linkedNPC != null)
            linkedNPC.Interact();
    }
}
