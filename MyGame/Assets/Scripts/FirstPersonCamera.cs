using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalClampMin = -90f;
    [SerializeField] private float verticalClampMax = 90f;

    private float xRotation = 0f;

    private void Start()
    {
        // Lock and hide cursor for first-person view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Read mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

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
