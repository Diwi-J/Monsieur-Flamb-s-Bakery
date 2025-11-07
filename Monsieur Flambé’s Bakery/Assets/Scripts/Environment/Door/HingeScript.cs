using UnityEngine;
using UnityEngine.InputSystem;

public class HingeDoor : MonoBehaviour
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

    [Header("NPC Requirement")]
    public NPC npcToTalkTo; // Assign the NPC that must be spoken to

    [Header("Feedback")]
    public string lockedMessage = "You need to speak to the NPC first!";

    private bool isOpen = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private bool playerInRange = false;
    private Transform player;

    private void Awake()
    {
        if (hinge == null)
        {
            hinge = transform.childCount > 0 ? transform.GetChild(0) : transform;
        }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("[HingeDoor] Player not found. Tag your player 'Player'.");

        if (interactAction == null)
            Debug.LogWarning("[HingeDoor] Interact action not assigned.");
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

        // NPC check: prevent opening if player hasn't spoken
        if (npcToTalkTo != null && !npcToTalkTo.HasBeenSpokenTo)
        {
            Debug.Log(lockedMessage);
            return;
        }

        // Determine swing direction
        Vector3 toPlayer = player.position - hinge.position;
        float side = Vector3.Dot(hinge.right, toPlayer);
        int swingDirection = side >= 0 ? 1 : -1;

        isOpen = !isOpen;
        targetAngle = isOpen ? swingDirection * openAngle : 0f;

        Debug.Log("Door opens!");
    }

    private void Update()
    {
        if (useProximityCheck && player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            playerInRange = dist <= proximityRadius;
        }

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

    // Call this manually or from NPC when unlocked
    public void TryOpenDoor()
    {
        if (npcToTalkTo != null && !npcToTalkTo.HasBeenSpokenTo)
        {
            Debug.Log(lockedMessage);
            return;
        }

        // Determine swing direction (default if player is null)
        Vector3 toPlayer = player != null ? player.position - hinge.position : Vector3.forward;
        float side = Vector3.Dot(hinge.right, toPlayer);
        int swingDirection = side >= 0 ? 1 : -1;

        isOpen = true;
        targetAngle = swingDirection * openAngle;

        Debug.Log("Door opens!");
    }
}
