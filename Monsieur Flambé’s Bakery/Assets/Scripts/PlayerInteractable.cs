using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public float interactRange = 10f;
    public Transform hand;
    private GameObject heldItem;

    public void TryPickUp()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (heldItem != null) DropItem();

                interactable.Interact();
                heldItem = hit.collider.gameObject;
            }
        }
    }

    public void DropItem()
    {
        if (heldItem == null) return;

        PickupItem pickup = heldItem.GetComponent<PickupItem>();
        if (pickup != null)
            pickup.Drop();

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }

        heldItem = null;
    }
}
