using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private FirstPersonCamera cameraController;

    private CharacterController characterController;
    private Vector2 moveInput;
    private bool isSprinting;
    private Vector3 velocity;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();

        // If camera controller not assigned, try to find it
        if (cameraController == null)
        {
            cameraController = GetComponentInChildren<FirstPersonCamera>();
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed;
    }

    private void Update()
    {
        MovePlayer();
        ApplyGravity();
    }

    private void MovePlayer()
    {
        // Get camera-relative directions
        Vector3 forward = Vector3.zero;
        Vector3 right = Vector3.zero;

        if (cameraController != null)
        {
            forward = cameraController.GetForwardDirection();
            right = cameraController.GetRightDirection();
        }
        else
        {
            // Fallback to transform directions if no camera
            forward = transform.forward;
            right = transform.right;
        }

        // Flatten directions to prevent flying when looking up/down
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate movement direction based on input and camera direction
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // Apply speed with sprint modifier
        float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);

        // Move the character
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        // Apply gravity
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep grounded
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
