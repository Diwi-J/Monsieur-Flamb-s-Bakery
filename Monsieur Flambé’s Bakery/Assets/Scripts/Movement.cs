using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 2f;
    public float gravity = 9.81f;
    public float lookSpeed = 0.5f;
    public float lookXLimit = 45f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private CharacterController characterController;

    private PlayerControls controls;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private bool isJumping = false;
    private bool isRunning = false;

    void Awake()
    {
        //Initialize components.
        characterController = GetComponent<CharacterController>();
        controls = new PlayerControls();

        //Movement controls.
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        //Look controls.
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        //Sprint controls.
        controls.Player.Sprint.performed += ctx => isRunning = true;
        controls.Player.Sprint.canceled += ctx => isRunning = false;
    }

    void OnEnable()
    {
        //Enable the controls and lock the cursor.
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        //Disable the controls and unlock the cursor.
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        //Handle movement and look input.
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 desiredMove = (forward * moveInput.y + right * moveInput.x) * speed;

        //Apply gravity.
        if (characterController.isGrounded)
        {
            moveDirection = desiredMove;

            if (isJumping)
            {
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            moveDirection.x = desiredMove.x;
            moveDirection.z = desiredMove.z;
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Move the character controller.
        characterController.Move(moveDirection * Time.deltaTime);

        //Look rotation.
        rotationX += -lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        transform.Rotate(Vector3.up * lookInput.x * lookSpeed);
    }
}
