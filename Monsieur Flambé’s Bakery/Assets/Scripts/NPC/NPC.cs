using UnityEngine;

public class NPC : Interactable
{
    [Header("Dialogue")]
    public Dialogue dialogue;               //Normal dialogue
    public Dialogue blockedDialogue;        //Dialogue shown if cake isn't placed

    [Header("Cake Requirement")]
    [Tooltip("Assign only if this NPC should wait for the cake")]
    public CakeTargetZone cakeTargetZone;

    public override void Interact()
    {
        //Check if this NPC is waiting for the cake
        if (cakeTargetZone != null && !cakeTargetZone.cakePlaced)
        {
            //Show blocked dialogue
            if (blockedDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(blockedDialogue);
            }
        }

        //Normal dialogue
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
