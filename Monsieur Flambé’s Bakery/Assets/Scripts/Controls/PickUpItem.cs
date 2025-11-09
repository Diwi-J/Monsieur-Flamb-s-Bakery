using UnityEngine;
using System.Collections;

public class PickupItem : Interactable
{
    private Rigidbody rb;
    private Collider col;
    private Vector3 originalScale;
    private Transform originalParent;
    private bool isHeld = false;
    private bool canBePickedUp = true;

    [Header("Hand Settings")]
    [SerializeField] private Transform holdParent;
    [SerializeField] private Vector3 holdOffset = new Vector3(0f, 0.1f, 0.2f);

    [Header("Held Appearance")]
    [SerializeField] private float handScaleMultiplier = 1f;

    [Header("Physics Settings")]
    [SerializeField] private float dropPhysicsDelay = 0.05f;

    public bool canPickUp = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalScale = transform.localScale;
        originalParent = transform.parent;
    }

    public override void Interact() { }

    public void PickUp()
    {
        //Overload to use default hold parent.
        PickUp(holdParent);
    }

    public void PickUp(Transform hand)
    {
        if (!canPickUp || hand == null || isHeld) return;

        isHeld = true;

        //Disable physics.
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.None;

        if (col != null)
            col.enabled = false;

        //Parent to hand.
        transform.SetParent(hand, worldPositionStays: false);
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale * handScaleMultiplier;

        Debug.Log($"Picked up {gameObject.name}");
    }

    private void LateUpdate()
    {
        //Maintain position and rotation while held.
        if (isHeld && holdParent != null)
        {
            transform.position = holdParent.position + holdParent.TransformVector(holdOffset);
            transform.rotation = holdParent.rotation;
        }
    }

    public void Drop()
    {
        //Drop the item.
        if (!isHeld) return;

        isHeld = false;
        canBePickedUp = false;

        //Unparent from hand.
        transform.SetParent(originalParent, worldPositionStays: true);
        transform.localScale = originalScale;
        transform.position += Vector3.up * 0.05f;

        StartCoroutine(EnablePhysicsAfterDelay());
    }

    private IEnumerator EnablePhysicsAfterDelay()
    {
        //Delay to avoid immediate collisions with the player.
        yield return new WaitForSeconds(dropPhysicsDelay);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        if (col != null)
            col.enabled = true;

        canBePickedUp = true;
        Debug.Log($"Dropped {gameObject.name}");
    }
}
