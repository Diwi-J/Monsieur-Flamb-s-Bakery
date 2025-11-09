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
    private PlayerInput input;

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

    private bool controlstate;
    private bool isRunning;

    [Header("Pause Menu")]
    public PauseMenu pauseMenu;

    private RecipeBookToggle focusedBook;

    private void Awake()
    {
        //Get components.
        controller = GetComponent<CharacterController>();
        interactable = GetComponent<PlayerInteractable>();
        input = GetComponent<PlayerInput>();
        controls = new PlayerControls();

        OnEnable();
    }
    private void Update()
    {
        //Disable controls when paused.
        if (controlstate) return;

        HandleMovement();
        HandleLook();

        //Check for focused recipe book.
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            focusedBook = hit.collider.GetComponent<RecipeBookToggle>();
        }
        else
        {
            focusedBook = null;
        }
    }

    #region Unity Events

    //Movement and Look.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();   
    }

    //Sprint.
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;

        }
        else if (context.canceled)
        {
            isRunning = false; 
        }
    }

    //Look.
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    //Interact.
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (focusedBook != null)
        {
            focusedBook.ToggleRecipeBook();
        }

        interactable.TryInteract();
    }

    //Pick Up/Drop.
    public void OnPickUp()
    {
        interactable.TryInteract();
    }

    public void OnDrop()
    {
        interactable.DropItem();
    }

    //Pause.
    public void OnPause()
    {
        pauseMenu.PauseGame();

        Debug.Log("Pausing");
    }

    #endregion

    #region controls Ontoggle
    public void OnEnable()
    {
        //controls.Enable();
        input.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controlstate = false;
    }

    public void OnDisable()
    {
        //controls.Disable();
        input.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        controlstate = true;
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
