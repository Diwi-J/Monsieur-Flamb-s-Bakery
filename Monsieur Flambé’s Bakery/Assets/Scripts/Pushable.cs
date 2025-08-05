using UnityEngine;

public class Pushable : Interactable
{
    public override void Interact()
    {
        Debug.Log("You can push this object!");
        // You don’t need extra physics code unless you want scripted pushing.
    }
}
