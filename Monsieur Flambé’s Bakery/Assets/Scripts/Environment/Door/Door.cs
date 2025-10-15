using UnityEngine;
using UnityEngine.InputSystem;

public class DoorNewInput : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactAction;

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float speed = 2f;
    public float snapThreshold = 0.5f;

    private bool isOpen = false;
    private bool playerInRange = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Freeze any weird rotation inheritance
        transform.localRotation = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f);

        // Define "closed" as the rotation at start
        closedRotation = transform.localRotation;

        // Define "open" relative to the closed state
        targetRotation = closedRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!playerInRange) return;

        isOpen = !isOpen;
        float targetY = isOpen ? openAngle : 0f;
        targetRotation = closedRotation * Quaternion.Euler(0f, targetY, 0f);
    }

    void Update()
    {
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * speed
        );

        if (Quaternion.Angle(transform.localRotation, targetRotation) < snapThreshold)
            transform.localRotation = targetRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
