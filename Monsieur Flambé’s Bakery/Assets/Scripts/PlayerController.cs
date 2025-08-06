using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform hand; // Drag the "Hand" object in the Inspector
    public GameObject heldItem;

    void Update()
    {
        // Check for item pickup
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            DropItem();
        }
    }

    void DropItem()
    {
        // Unparent the item from the hand and reset its position and rotation
        heldItem.transform.SetParent(null);

        // Reset the item's position and rotation to its original state
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(transform.forward * 3f, ForceMode.Impulse); // Optional "throw"
        }
        
        heldItem = null;
    }

}
