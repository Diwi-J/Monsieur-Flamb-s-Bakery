using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public Transform playerCamera;
    public float moveSpeed = 5f;
    public float lookSensitivity = 1.5f;
    public float lookXLimit = 45f;

    [Header("Interaction")]
    public MixingBowl nearbyBowl;

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotationX = 0f;

    private PlayerControls controls;
    private PlayerInteractable interactable;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        interactable = GetComponent<PlayerInteractable>();
        controls = new PlayerControls();

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Look
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Interact (Pickup/Other Interactables)
        controls.Player.Interact.performed += ctx => interactable.TryInteract();

        // Drop (optional separate key)
        controls.Player.Drop.performed += ctx => interactable.DropItem();

        //Mix
        /*controls.Player.PlayerMix.performed += ctx => interactable.TryInteract();*/
        
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        Vector3 move = (forward + right) * moveSpeed;
        characterController.Move(move * Time.deltaTime);
    }

    private void HandleLook()
    {
        rotationX -= lookInput.y * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }
}
