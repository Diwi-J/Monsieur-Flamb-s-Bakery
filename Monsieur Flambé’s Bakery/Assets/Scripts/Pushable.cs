using UnityEngine;

public class Pushable : Interactable
{
    public override void Interact()
    {
        // Check if the player is holding an item
        Debug.Log("You can push this object!");

    }
}
