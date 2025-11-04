using UnityEngine;
using UnityEngine.InputSystem; // Needed for the new input system

public class DoorWithNewInput : MonoBehaviour
{
    [Header("References")]
    public Transform doorMesh;

    [Header("Settings")]
    public float openAngle = 90f;
    public float openSpeed = 6f;
    public bool useProximityCheck = false;
    public float proximityRadius = 2f;

    [Header("Input")]
    [Tooltip("Your player's InputActionAsset or PlayerInput component must have an 'Interact' action.")]
    public InputActionReference interactAction;

    private Quaternion closedLocalRot;
    private Quaternion openLocalRot;
    private bool isOpen = false;
    private bool playerInRange = false;
    private Transform player;

    private void Awake()
    {
        if (doorMesh == null) doorMesh = transform;

        closedLocalRot = doorMesh.localRotation;
        openLocalRot = closedLocalRot * Quaternion.Euler(0f, openAngle, 0f);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("[DoorWithNewInput] Player not found. Tag your player as 'Player'.");

        if (interactAction == null)
            Debug.LogWarning("[DoorWithNewInput] Missing InputActionReference for 'Interact'.");
    }

    private void OnEnable()
    {
        if (interactAction != null)
            interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.action.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (playerInRange)
        {
            isOpen = !isOpen;
        }
    }

    private void Update()
    {
        // Optional distance check instead of trigger
        if (useProximityCheck && player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            playerInRange = dist <= proximityRadius;
        }

        Quaternion targetRot = isOpen ? openLocalRot : closedLocalRot;
        doorMesh.localRotation = Quaternion.Slerp(doorMesh.localRotation, targetRot, Time.deltaTime * openSpeed);
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

    private void OnDrawGizmosSelected()
    {
        if (useProximityCheck)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, proximityRadius);
        }
    }
}
