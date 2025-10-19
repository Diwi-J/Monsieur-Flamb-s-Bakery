using UnityEngine;

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
        originalParent = transform.parent;
    }

    //Override the Interact method to pick up or drop the item
    public override void Interact()
    {
        if (!canPickUp) return;

        if (!isHeld)
            PickUp(holdParent);
        else
            Drop();
    }

    //Pick up the item into the specified hand transform
    public void PickUp(Transform hand)
    {
        if (!canPickUp || hand == null || isHeld) return;

        isHeld = true;

        originalParent = transform.parent;
        originalScale = transform.lossyScale;

        rb.useGravity = false;
        rb.isKinematic = true;

        transform.SetParent(hand, worldPositionStays: true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //Apply safe hand scale multiplier
        Vector3 parentScale = hand.lossyScale;
        transform.localScale = new Vector3(
            originalScale.x * handScaleMultiplier / parentScale.x,
            originalScale.y * handScaleMultiplier / parentScale.y,
            originalScale.z * handScaleMultiplier / parentScale.z
        );
    }

    //Pick up the item into the default hold parent
    public void PickUp()
    {
        PickUp(holdParent);
    }

    //Drop the item from the hand
    public void Drop()
    {
        if (!isHeld) return;

        isHeld = false;

        transform.SetParent(originalParent, worldPositionStays: true);

        rb.isKinematic = false;
        rb.useGravity = true;

        transform.localScale = originalScale;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
