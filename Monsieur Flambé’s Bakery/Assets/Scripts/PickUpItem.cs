using UnityEngine;

public class PickupItem : Interactable
{
    private Vector3 originalScale;

    private void Awake()
    {
        // Save the object's original size at the beginning
        originalScale = transform.localScale;
    }

    public override void Interact()
    {
        // Find the player's hand transform using the PlayerInteractable script
        Transform hand = GameObject.FindWithTag("Player")?.GetComponent<PlayerInteractable>()?.hand;

        // Check if the hand transform is found
        if (hand == null)
        {
            Debug.LogError("Hand not found!");
            return;
        }

        // If the item is already held, do nothing
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Set the item's parent to the hand transform
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Resize for holding
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    public void Drop()
    {
        // Unparent the item from the hand and reset its position and rotation
        transform.SetParent(null);

        // Reset the item's position and rotation to its original state
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.linearDamping = 5f;
            rb.angularDamping = 10f;
            rb.constraints = RigidbodyConstraints.None; // Or freeze some axes

        }

        // Restore original size
        transform.localScale = originalScale;
    }
}
