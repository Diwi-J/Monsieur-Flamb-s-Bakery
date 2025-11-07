using UnityEngine;

public class CakeInspectorNPC : Interactable
{
    public CakeTargetZone cakeTargetZone;
    public NPC linkedNPC; // normal NPC dialogue

    public override void Interact()
    {
        if (cakeTargetZone != null && !cakeTargetZone.IsCakePlaced())
        {
            Debug.Log("I can't talk to you until the cake is placed!");
            return;
        }

        if (linkedNPC != null)
            linkedNPC.Interact();
    }
}
