using UnityEngine;
using UnityEngine.InputSystem;

public class HingeScript : MonoBehaviour
{
    [Header("References")]
    public Transform hinge;

    [Header("Door Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float openAngle = 90f;
    public float speed = 5f;

    [Header("Player Detection")]
    public bool useProximityCheck = false;
    public float proximityRadius = 2f;

    [Header("Input")]
    public InputActionReference interactAction;

    private bool isOpen = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private bool playerInRange = false;
    private Transform player;

    private void Awake()
    {
        // Auto-assign hinge if none
        if (hinge == null)
        {
            if (transform.childCount > 0)
                hinge = transform.GetChild(0);
            else
                hinge = transform;
        }

        // Find player by tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (interactAction == null)
            Debug.LogWarning("Interact action not assigned.");
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteract;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!playerInRange || player == null) return;

        // Determine side to swing based on player position
        Vector3 toPlayer = player.position - hinge.position;
        float side = Vector3.Dot(hinge.right, toPlayer);
        int swingDirection = side >= 0 ? 1 : -1;

        isOpen = !isOpen;
        targetAngle = isOpen ? swingDirection * openAngle : 0f;
    }

    private void Update()
    {
        // Optional proximity detection
        if (useProximityCheck && player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            playerInRange = dist <= proximityRadius;
        }

        // Smooth rotation
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * speed);
        hinge.localRotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useProximityCheck && other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!useProximityCheck && other.CompareTag("Player"))
            playerInRange = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (useProximityCheck)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, proximityRadius);
        }
    }
}
