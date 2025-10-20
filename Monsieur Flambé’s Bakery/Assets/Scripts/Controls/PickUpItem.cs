using UnityEngine;

public class PickupItem : Interactable
{
    private Rigidbody rb;
    private bool isHeld = false;

    [Header("Hand Settings")]
    public Transform holdParent;          // Default hand parent
    public Vector3 handWorldScale = Vector3.one; // Desired world scale in hand

    [Header("Pickup Control")]
    public bool canPickUp = true;        // Whether the item can be picked up

    private Vector3 originalLocalScale;
    private Vector3 originalWorldScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalLocalScale = transform.localScale;
        originalWorldScale = transform.lossyScale; // world scale
    }

    public override void Interact()
    {
        if (!canPickUp) return;

        if (!isHeld)
            PickUp();
        else
            Drop();
    }

    public void PickUp()
    {
        if (!canPickUp || holdParent == null) return;
        DoPickUp(holdParent);
    }

    public void PickUp(Transform customParent)
    {
        if (!canPickUp || customParent == null) return;
        DoPickUp(customParent);
    }

    private void DoPickUp(Transform parent)
    {
        isHeld = true;

        rb.useGravity = false;
        rb.isKinematic = true;

        // Parent first
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Apply desired world scale regardless of parent's scale
        Vector3 parentScale = parent.lossyScale;
        transform.localScale = new Vector3(
            handWorldScale.x / parentScale.x,
            handWorldScale.y / parentScale.y,
            handWorldScale.z / parentScale.z
        );
    }

    public void Drop()
    {
        isHeld = false;

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Restore original local scale
        transform.localScale = originalLocalScale;
    }
}
