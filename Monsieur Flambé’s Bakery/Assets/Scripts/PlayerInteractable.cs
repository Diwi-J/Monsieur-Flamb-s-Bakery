using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public float interactRange = 5f;
    public Transform hand;
    public PickupItem heldItem;

    public void TryInteract()
    {
        // Drop if holding an item
        if (heldItem != null)
        {
            DropItem();
            return;
        }

        // Raycast to pick up nearby item
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            PickupItem pickup = hit.collider.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.PickUp(hand);
                heldItem = pickup;
                Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red, 2f);
                Debug.Log("Trying to interact: " + hit.collider?.name);

            }
            else
            {
                // For other interactables
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                    interactable.Interact();
            }

        }
    }

    public void DropItem()
    {
        if (heldItem == null) return;

        heldItem.Drop();
        heldItem = null;
    }
}

