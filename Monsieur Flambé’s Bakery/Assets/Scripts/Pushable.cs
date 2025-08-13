using UnityEngine;

public class Pushable : MonoBehaviour
{
    public float pushPower = 2.0f;

    private Rigidbody rb;

    void Start()
    {
        //Ensure the object has a Rigidbody component
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("Pushable object requires a Rigidbody.");
        }
    }

    public void Push(Vector3 pushDirection)
    {
        if (rb != null)
        {
            //Only push horizontally — zero out Y to avoid flying
            Vector3 force = new Vector3(pushDirection.x, 0, pushDirection.z) * pushPower;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
