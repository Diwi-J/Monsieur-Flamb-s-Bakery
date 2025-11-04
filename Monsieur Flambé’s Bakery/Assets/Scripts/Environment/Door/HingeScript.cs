using UnityEngine;
using UnityEngine.InputSystem;

public class SmartHingeDoor : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactAction;

    [Header("Door Settings")]
    public Transform hinge;           // parent object to rotate
    public Vector3 rotationAxis = Vector3.up;
    public float openAngle = 90f;
    public float speed = 3f;

    private bool isOpen = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private bool playerInRange = false;
    private Transform player;

    private void Awake()
    {
        if (hinge == null)
            hinge = transform.GetChild(0); // auto-find first child if none assigned

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("[SmartHingeDoor] Player not found. Tag your player 'Player'.");
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

        // Determine side: vector from hinge to player
        Vector3 toPlayer = player.position - hinge.position;

        // Calculate perpendicular to hinge forward to get swing side
        float side = Vector3.Dot(hinge.right, toPlayer); // hinge.right points to door’s right edge
        int swingDirection = side >= 0 ? 1 : -1;

        isOpen = !isOpen;
        targetAngle = isOpen ? swingDirection * openAngle : 0f;
    }

    private void Update()
    {
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * speed);
        hinge.localRotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
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
