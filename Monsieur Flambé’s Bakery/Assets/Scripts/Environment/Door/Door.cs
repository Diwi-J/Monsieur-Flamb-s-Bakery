using UnityEngine;
using UnityEngine.InputSystem;

public class DoorNewInput : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactAction;

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private float targetAngle = 0f;
    private bool playerInRange = false;

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

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerInRange) return; //only interact if player is near this door

        isOpen = !isOpen;
        targetAngle = isOpen ? openAngle : 0f;
    }

    void Update()
    {
        float currentY = transform.localEulerAngles.y;
        float newY = Mathf.LerpAngle(currentY, targetAngle, Time.deltaTime * speed);
        transform.localEulerAngles = new Vector3(0, newY, 0);
    }

    //Player enters door trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    //Player leaves door trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
