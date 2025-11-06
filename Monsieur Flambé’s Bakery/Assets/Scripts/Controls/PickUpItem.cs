using UnityEngine;
using System.Collections;

public class PickupItem : Interactable
{
    private Rigidbody rb;
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
        originalScale = transform.localScale;

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
        originalScale = transform.localScale;

        // Start safe pickup coroutine
        StartCoroutine(PickupRoutine(hand));
    }

    private IEnumerator PickupRoutine(Transform hand)
    {
        // Freeze physics first
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        // Wait for end of frame to avoid physics jumps
        yield return new WaitForEndOfFrame();

        // Parent to hand
        transform.SetParent(hand, worldPositionStays: true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Apply safe scale
        Vector3 parentScale = hand.lossyScale;
        transform.localScale = new Vector3(
            originalScale.x * handScaleMultiplier / parentScale.x,
            originalScale.y * handScaleMultiplier / parentScale.y,
            originalScale.z * handScaleMultiplier / parentScale.z
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
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Restore scale
        transform.localScale = originalScale;

        // Small offset to prevent sinking
        transform.position += Vector3.up * 0.05f;

        Debug.Log("Dropped " + gameObject.name);
    }
}
