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
        //Drop if players already holding an item.
        if (heldItem != null)
        {
            DropItem();
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactRange))
        {
            //Check for PickupItem first.
            PickupItem pickup = hit.collider.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.PickUp(hand);
                heldItem = pickup;
                return;
            }

            //Other interactables.
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void DropItem()
    {
        //Ensure there is an item to drop.
        if (heldItem == null) return;

        heldItem.Drop();
        heldItem = null;
    }

    //Just to help visualize the interaction range in the editor.
    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * interactRange);
        Gizmos.DrawWireSphere(playerCamera.position + playerCamera.forward * interactRange, sphereRadius);
    }
}