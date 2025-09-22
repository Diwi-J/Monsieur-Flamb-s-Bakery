using UnityEngine;
using UnityEngine.InputSystem;

public class HingeDoor : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactAction;

    [Header("Door Settings")]
    public Transform hinge;           
    public Vector3 rotationAxis = Vector3.up; 
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private bool playerInRange = false;

    void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteract;
        }
    }

    void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerInRange) return;

        isOpen = !isOpen;
        targetAngle = isOpen ? openAngle : 0f;
    }

    void Update()
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
