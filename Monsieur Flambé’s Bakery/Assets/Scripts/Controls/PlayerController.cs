using Unity.VisualScripting;
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

    public PauseMenu pauseMenu;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        interactable = GetComponent<PlayerInteractable>();
        controls = new PlayerControls();

        //Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        //Look
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        //Interact (Pickup/Other Interactables)
        controls.Player.Interact.performed += ctx => interactable.TryInteract();

        //Drop
        controls.Player.Drop.performed += ctx => interactable.DropItem();

        //Mix
        //Might use at a later stage for players to mix ingredients themselves.
        /*controls.Player.PlayerMix.performed += ctx => interactable.TryInteract();*/

        //Pause 
        controls.UI.Pause.performed += ctx => pauseMenu.PauseGame();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    //Handles player movement.
    private void HandleMovement()
    {
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        Vector3 move = (forward + right) * moveSpeed;
        characterController.Move(move * Time.deltaTime);
    }

    //Handles player looking around with the camera.
    private void HandleLook()
    {
        rotationX -= lookInput.y * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }

    #region Intput Action System 
    /*
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;

    private float verticalRotation = 0f;

    private void Awake()
    {
        //Initialize the CharacterController and lock the cursor
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    //Input System Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnPause(InputAction.CallBackContext context)
    {
        
    }

    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward *
        moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit,
        verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    */
    #endregion 
}

