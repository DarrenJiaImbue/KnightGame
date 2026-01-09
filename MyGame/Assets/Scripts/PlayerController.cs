using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpPressed;

    private Vector2 moveInput;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckGround();
        HandleGravity();
        HandleJump();
        HandleMovement();
    }

    private void CheckGround()
    {
        // Check if player is on the ground using a small sphere at the player's feet
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            // Fallback to CharacterController's built-in ground check
            isGrounded = controller.isGrounded;
        }

        // Reset vertical velocity when grounded to prevent accumulation
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
    }

    private void HandleGravity()
    {
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        // Jump only if grounded and jump was pressed (prevents double-jumping)
        if (jumpPressed && isGrounded)
        {
            // Calculate jump velocity using physics formula: v = sqrt(2 * h * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Reset jump input after processing
        jumpPressed = false;
    }

    private void HandleMovement()
    {
        // Apply movement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply vertical velocity (gravity and jump)
        controller.Move(velocity * Time.deltaTime);
    }

    // Input System callback for Jump action
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
    }

    // Input System callback for Move action
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
