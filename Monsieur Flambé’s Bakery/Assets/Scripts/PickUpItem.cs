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
        Transform hand = GameObject.FindWithTag("Player")?.GetComponent<PlayerInteractable>()?.hand;

        if (hand == null)
        {
            Debug.LogError("Hand not found!");
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Resize for holding
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    public void Drop()
    {
        transform.SetParent(null);

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
