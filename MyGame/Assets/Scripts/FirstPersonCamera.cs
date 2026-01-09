using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalClampMin = -90f;
    [SerializeField] private float verticalClampMax = 90f;

    private float xRotation = 0f;
    private Vector2 lookInput;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        // Lock and hide cursor for first-person view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Disable();

        // Unlock cursor when disabled
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Calculate rotation amounts
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Rotate camera vertically (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, verticalClampMin, verticalClampMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player body horizontally (yaw)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public Vector3 GetForwardDirection()
    {
        if (playerBody != null)
        {
            return playerBody.forward;
        }
        return transform.forward;
    }

    public Vector3 GetRightDirection()
    {
        if (playerBody != null)
        {
            return playerBody.right;
        }
        return transform.right;
    }
}
