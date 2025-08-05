using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    public Transform hand; // Drag the Hand GameObject in Inspector
    private GameObject heldItem;

    void Update()
    {
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
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // If already holding something, drop it first
                if (heldItem != null)
                {
                    DropItem();
                }

                interactable.Interact();

                // Save reference to the new held item
                heldItem = hit.collider.gameObject;
            }
        }
    }

    void DropItem()
    {
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


        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero; // Reset motion
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse); // Optional "throw"
        }

        heldItem = null;
    }
}
