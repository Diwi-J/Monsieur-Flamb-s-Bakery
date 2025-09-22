using UnityEngine;

public class CakeInspectorNPC : Interactable
{
    public CakeTargetZone cakeTargetZone; 

    public override void Interact()
    {
        if (cakeTargetZone != null && !cakeTargetZone.cakePlaced)
        {
            Debug.Log("I can't talk to you until the cake is placed!");
            return;
        }
    }
}
