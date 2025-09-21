using UnityEngine;

public class PickupItem : Interactable
{
    private Vector3 originalScale;
    private Rigidbody rb;

    [Header("Hand Settings")]
    [SerializeField] private float handScaleFactor = 0.7f; // scale in hand

    private void Awake()
    {
        //Initialize original scale and Rigidbody.
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    //Pick up the item by parenting it to the hand transform.
    public void PickUp(Transform hand)
    {
        if (hand == null)
        {
            Debug.LogWarning($"No hand assigned for {gameObject.name}");
            return;
        }

        //Makes it kinematic, disables gravity.
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        //Parent to hand and adjust transform.
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale * handScaleFactor;
    }

    public void Drop()
    {
        //Removes parent.
        transform.SetParent(null);
        transform.localScale = originalScale;

        //Restores physics.
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public override void Interact()
    {
        //Just incase the Interact() stops working.
        Debug.Log($"Interacted with {gameObject.name}");
    }
}

