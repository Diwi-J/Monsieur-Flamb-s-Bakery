using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Transform hand; // Reference to the player's hand transform
    public GameObject heldItem; // Reference to the object being held, if any
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
