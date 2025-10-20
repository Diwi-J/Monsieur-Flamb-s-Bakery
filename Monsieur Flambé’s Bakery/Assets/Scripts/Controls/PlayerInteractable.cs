using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float interactRange = 3f;
    public float sphereRadius = 0.3f;
    public Transform hand;
    public PickupItem heldItem;
    public Transform playerCamera;

    public void TryInteract()
    {
        // Drop if the player is already holding an item
        if (heldItem != null)
        {
            DropItem();
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactRange))
        {
            // Check for PickupItem first
            PickupItem pickup = hit.collider.GetComponent<PickupItem>();
            if (pickup != null && pickup.canPickUp) // ✅ Respect canPickUp
            {
                // Use hand if assigned, otherwise fallback to PickupItem default
                if (hand != null)
                    pickup.PickUp(hand);
                else
                    pickup.PickUp();

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

    // Visualize interaction range in editor
    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * interactRange);
        Gizmos.DrawWireSphere(playerCamera.position + playerCamera.forward * interactRange, sphereRadius);
    }
}
