using UnityEngine;

public class CakeInspectorNPC : Interactable
{
    public CakeTargetZone cakeTargetZone;
    public NPC linkedNPC; //Main NPC dialogue

    public override void Interact()
    {
        //Check if cake is placed in the target zone.
        if (cakeTargetZone != null && !cakeTargetZone.IsCakePlaced())
        {
            Debug.Log("I can't talk to you until the cake is placed!");
            return;
        }

        if (linkedNPC != null)
            linkedNPC.Interact();
    }
}
