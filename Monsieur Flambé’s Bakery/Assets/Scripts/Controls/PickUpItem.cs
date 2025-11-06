using UnityEngine;
using System.Collections;

public class PickupItem : Interactable
{
    private Rigidbody rb;
    private Collider col;
    private Vector3 originalScale;
    private Transform originalParent;
    private bool isHeld = false;

    [Header("Hand Settings")]
    [SerializeField] private Transform holdParent;

    [Header("Held Appearance")]
    [Tooltip("Optional: scale multiplier applied when held. 1 = same size as prefab.")]
    [SerializeField] private float handScaleMultiplier = 1f;

    public bool canPickUp = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalScale = transform.localScale; // Store once here
        if (transform.parent != null)
            originalParent = transform.parent;
    }

    public override void Interact()
    {
        Debug.Log("Interact called on " + gameObject.name);

        if (!canPickUp) return;

        if (!isHeld)
            PickUp(holdParent);
        else
            Drop();
    }

    public void PickUp(Transform hand)
    {
        if (!canPickUp)
        {
            Debug.Log("Cannot pick up " + gameObject.name + " because canPickUp is false");
            return;
        }

        if (hand == null)
        {
            Debug.LogWarning("Hand is null for " + gameObject.name);
            return;
        }

        if (isHeld) return;

        isHeld = true;

        // Start safe pickup coroutine
        StartCoroutine(PickupRoutine(hand));
    }

    private IEnumerator PickupRoutine(Transform hand)
    {
        // --- FIX: freeze physics and disable collider to prevent collision issues ---
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        if (col != null)
            col.enabled = false; // Disable collisions while held

        // Wait a frame to stabilize
        yield return null;

        // --- Parent with worldPositionStays=false so it snaps to hand ---
        transform.SetParent(hand, worldPositionStays: false);

        // --- Reset local position and rotation to align perfectly ---
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Apply safe scale
        Vector3 parentScale = hand.lossyScale;
        transform.localScale = new Vector3(
            originalScale.x * handScaleMultiplier / Mathf.Max(parentScale.x, 0.0001f),
            originalScale.y * handScaleMultiplier / Mathf.Max(parentScale.y, 0.0001f),
            originalScale.z * handScaleMultiplier / Mathf.Max(parentScale.z, 0.0001f)
        );

        Debug.Log("Picked up " + gameObject.name);
    }

    public void PickUp()
    {
        PickUp(holdParent);
    }

    public void Drop()
    {
        if (!isHeld) return;

        isHeld = false;

        // Restore parent safely
        transform.SetParent(originalParent, worldPositionStays: true);

        // Restore physics
        rb.isKinematic = false;
        rb.useGravity = true;

        if (col != null)
            col.enabled = true; // Re-enable collisions

        // Restore scale
        transform.localScale = originalScale;

        // Small offset to prevent sinking
        transform.position += Vector3.up * 0.05f;

        Debug.Log("Dropped " + gameObject.name);
    }
}
