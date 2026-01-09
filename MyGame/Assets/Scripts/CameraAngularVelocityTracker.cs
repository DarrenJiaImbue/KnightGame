using UnityEngine;

/// <summary>
/// Tracks the angular velocity of the camera based on its rotation changes.
/// Attach this to the main camera to detect how fast the camera is rotating.
/// </summary>
public class CameraAngularVelocityTracker : MonoBehaviour
{
    [Header("Tracking Settings")]
    [Tooltip("Smoothing factor for angular velocity (0-1, lower = smoother)")]
    [Range(0f, 1f)]
    public float smoothing = 0.3f;

    // Public read-only properties
    public Vector3 AngularVelocity { get; private set; }
    public float AngularSpeed { get; private set; }

    // Private tracking variables
    private Quaternion previousRotation;
    private Vector3 smoothedAngularVelocity;

    void Start()
    {
        previousRotation = transform.rotation;
        smoothedAngularVelocity = Vector3.zero;
    }

    void Update()
    {
        // Calculate rotation delta
        Quaternion currentRotation = transform.rotation;
        Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);

        // Convert to angular velocity (degrees per second)
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);

        // Normalize angle to -180 to 180 range
        if (angle > 180f)
            angle -= 360f;

        // Calculate angular velocity
        Vector3 angularVelocity = axis * angle / Time.deltaTime;

        // Handle NaN cases (can occur when axis is zero)
        if (float.IsNaN(angularVelocity.x) || float.IsNaN(angularVelocity.y) || float.IsNaN(angularVelocity.z))
        {
            angularVelocity = Vector3.zero;
        }

        // Smooth the angular velocity
        smoothedAngularVelocity = Vector3.Lerp(smoothedAngularVelocity, angularVelocity, smoothing);

        // Update public properties
        AngularVelocity = smoothedAngularVelocity;
        AngularSpeed = smoothedAngularVelocity.magnitude;

        // Store current rotation for next frame
        previousRotation = currentRotation;
    }

    /// <summary>
    /// Gets the angular velocity in a specific direction (useful for sword swinging in specific axes)
    /// </summary>
    public float GetAngularVelocityInDirection(Vector3 direction)
    {
        return Vector3.Dot(AngularVelocity, direction.normalized);
    }
}
