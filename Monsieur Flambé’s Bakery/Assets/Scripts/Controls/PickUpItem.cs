using UnityEngine;

public class PickupItem : Interactable
{
    private Vector3 originalScale;
    private Rigidbody rb;

    [Header("Hand Settings")]
    [SerializeField] private float handScaleFactor = 0.7f;

    private void Awake()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
    }

    //Pick up the item and attach to hand
    public void PickUp(Transform hand)
    {
        if (hand == null)
        {
            Debug.LogWarning("No hand assigned for {gameObject.name}");
            return;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale * handScaleFactor;
    }


    //Drop the item and restore original scale
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


    //Interaction logic (for demonstration)
    public override void Interact()
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }
}
