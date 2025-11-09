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
    public NPC npcToTalkTo; 

    [Header("Feedback")]
    public string lockedMessage = "You need to speak to the NPC first!";

    private bool isOpen = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private bool playerInRange = false;
    private Transform player;

    private void Awake()
    {
        //Default hinge to first child if not assigned.
        if (hinge == null)
        {
            hinge = transform.childCount > 0 ? transform.GetChild(0) : transform;
        }

        //Find player by tag.
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("HingeDoor Player not found.");

        if (interactAction == null)
            Debug.LogWarning("HingeDoor Interact action not assigned.");
    }

    private void OnEnable()
    {
        //Enable input action.
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteract;
        }
    }

    private void OnDisable()
    {
        //Disable input action.
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!playerInRange || player == null) return;

        //NPC check, prevent opening if player hasn't spoken.
        if (npcToTalkTo != null && !npcToTalkTo.HasBeenSpokenTo)
        {
            Debug.Log(lockedMessage);
            return;
        }

        //Determine swing direction.
        Vector3 toPlayer = player.position - hinge.position;
        float side = Vector3.Dot(hinge.right, toPlayer);
        int swingDirection = side >= 0 ? 1 : -1;

        isOpen = !isOpen;
        targetAngle = isOpen ? swingDirection * openAngle : 0f;

        Debug.Log("Door opens!");
    }

    private void Update()
    {
        //Proximity check.
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
        //Proximity trigger check.
        if (!useProximityCheck && other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnDrawGizmosSelected()
    {
        //Visualize proximity radius.
        if (useProximityCheck)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, proximityRadius);
        }
    }

    public void TryOpenDoor()
    {
        //NPC check (Check that player has spoken to NPC in order to open door).
        if (npcToTalkTo != null && !npcToTalkTo.HasBeenSpokenTo)
        {
            Debug.Log(lockedMessage);
            return;
        }

        //Determine swing direction to where player is.
        Vector3 toPlayer = player != null ? player.position - hinge.position : Vector3.forward;
        float side = Vector3.Dot(hinge.right, toPlayer);
        int swingDirection = side >= 0 ? 1 : -1;

        isOpen = true;
        targetAngle = swingDirection * openAngle;

        Debug.Log("Door opens!");
    }
}
