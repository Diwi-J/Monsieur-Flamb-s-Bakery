using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float interactRange = 3f;
    public float sphereRadius = 0.3f; // radius of SphereCast
    public Transform hand;
    public PickupItem heldItem;
    public Transform playerCamera; // assign your camera here

    public void TryInteract()
    {
        // Drop if holding an item
        if (heldItem != null)
        {
            DropItem();
            return;
        }

        // SphereCast from camera forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactRange))
        {
            PickupItem pickup = hit.collider.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.PickUp(hand);
                heldItem = pickup;
                return;
            }

            // Other interactables
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
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

    // Optional: debug visualization
    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * interactRange);
        Gizmos.DrawWireSphere(playerCamera.position + playerCamera.forward * interactRange, sphereRadius);
    }
}


