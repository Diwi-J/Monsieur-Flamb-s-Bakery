using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    private CharacterController controller;
    private PlayerControls controls;
    private PlayerInteractable interactable;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public float lookSensitivity = 1.5f;
    public float lookXLimit = 90f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalVelocity;
    private float rotationX;

    private bool isRunning;

    [Header("Pause Menu")]
    public PauseMenu pauseMenu;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        interactable = GetComponent<PlayerInteractable>();
        controls = new PlayerControls();

        OnEnable();
    }
    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    #region Unity Events

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();   
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;

            //Debug.Log("Sprinting");
        }
        else if (context.canceled)
        {
            isRunning = false; 
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnInteract()
    {
        interactable.TryInteract();
    }

    public void OnDrop()
    {
        interactable.DropItem();
    }

    public void OnPause()
    {
        pauseMenu.PauseGame();

        Debug.Log("Pausing");
    }

    #endregion

    #region controls Ontoggle
    public void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnDisable()
    {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion 

    #region Handle
    private void HandleMovement()
    {
        //Get move direction
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isRunning ? runSpeed : walkSpeed;

        //Vertical movement (gravity)
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }


        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed + Vector3.up * verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        rotationX -= lookInput.y * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }
    #endregion
}
