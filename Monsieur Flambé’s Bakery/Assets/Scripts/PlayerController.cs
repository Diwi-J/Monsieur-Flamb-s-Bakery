using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform hand; // Drag the "Hand" object in the Inspector
    public GameObject heldItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            DropItem();
        }
    }

    void DropItem()
    {
        heldItem.transform.SetParent(null);

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
