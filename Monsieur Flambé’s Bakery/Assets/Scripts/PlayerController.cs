
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform hand;                // Reference to the hand transform where items are held
    public KeyCode dropKey = KeyCode.Q;   // Key to drop the held item

    [Header("Held Item")]
    public GameObject heldItem;

    [Header("Throw Settings")]
    public float throwForce = 5f;
    public float upwardForce = 3f;

    void Update()
    {
        if (Input.GetKeyDown(dropKey) && heldItem != null)
        {
            DropHeldItem();
        }

    }

    private void DropHeldItem()
    {
        // Unparent the item from the hand
        heldItem.transform.SetParent(null);

        // Enable physics
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // Apply force in the forward + upward direction to simulate a throw
            Vector3 throwDirection = transform.forward * throwForce + transform.up * upwardForce;
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }

        // Clear reference
        heldItem = null;
    }

}
