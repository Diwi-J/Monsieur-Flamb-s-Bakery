using UnityEngine;

public class NPC : Interactable
{
    public Dialogue dialogue;

    public override void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
