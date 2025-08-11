using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public Transform hand;
    public Transform playerCamera;
    public float throwForce = 5f;
    public float upwardForce = 3f;
    public float lookSensitivity = 1.5f;
    public float lookXLimit = 45f;
    public float interactRange = 5f;

    private PlayerControls controls;
    private CharacterController characterController;

    private GameObject heldItem;
    private float rotationX = 0f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool isJumping;
    private bool isRunning;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        controls = new PlayerControls();

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Look
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Jump
        controls.Player.Jump.performed += ctx => isJumping = true;
        controls.Player.Jump.canceled += ctx => isJumping = false;

        // Sprint
        controls.Player.Sprint.performed += ctx => isRunning = true;
        controls.Player.Sprint.canceled += ctx => isRunning = false;

        // Interact
        controls.Player.Interact.performed += ctx => Interact();

        // Drop
        controls.Player.Drop.performed += ctx => DropHeldItem();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        Vector3 move = forward + right;
        // TODO: Apply movement with CharacterController.Move()
    }

    private void Interact()
    {
        Debug.Log("Interact pressed");

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (heldItem != null) DropHeldItem();

                interactable.Interact();
                heldItem = hit.collider.gameObject;
            }
        }
    }

    private void DropHeldItem()
    {
        if (heldItem == null) return;

        heldItem.transform.SetParent(null);

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            Vector3 throwDirection = transform.forward * throwForce + transform.up * upwardForce;
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }

        heldItem = null;
        Debug.Log("Dropped held item");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Pushable pushable = hit.collider.GetComponent<Pushable>();
        if (pushable != null)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            pushable.Push(pushDir);
        }
    }
}
