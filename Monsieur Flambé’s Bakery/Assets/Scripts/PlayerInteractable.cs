using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    public Transform hand;
    private GameObject heldItem;

    void Update()
    {
        // Check for interaction input
        if (Input.GetKeyDown(interactKey))
        {
            TryPickUp();
        }

        if (Input.GetKeyDown(dropKey) && heldItem != null)
        {
            DropItem();
        }
    }

    void TryPickUp()
    {

        // Cast a ray from the player's position in the forward direction to detect interactable objects
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            // Check if the hit object has an Interactable component
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // If already holding something, drop it first
                if (heldItem != null)
                {
                    DropItem();
                }

                interactable.Interact();

                // Set the held item to the interactable object
                heldItem = hit.collider.gameObject;
            }
        }
    }

    void DropItem()
    {
        // Unparent the item from the hand and reset its position and rotation
        PickupItem pickupScript = heldItem.GetComponent<PickupItem>();
        if (pickupScript != null)
        {
            pickupScript.Drop();
        }
        else
        {
            // fallback in case it's not a PickupItem
            heldItem.transform.SetParent(null);
        }

        // Reset the item's position and rotation to its original state
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero; // Reset motion
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse); // Throw
        }

        heldItem = null;
    }
}
