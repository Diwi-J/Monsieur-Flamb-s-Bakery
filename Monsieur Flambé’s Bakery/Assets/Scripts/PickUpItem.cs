using UnityEngine;

public class PickupItem : Interactable
{
    private Vector3 originalScale;
    private Rigidbody rb;

    private void Awake()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform hand)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale * 0.6f; // smaller in hand
    }

    public void Drop()
    {
        transform.SetParent(null);
        transform.localScale = originalScale;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public override void Interact()
    {
        // Not used now because PlayerInteractable handles pickup
        Debug.Log($"Interacted with {gameObject.name}");
    }
}
