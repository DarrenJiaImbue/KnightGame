using UnityEngine;

/// <summary>
/// Controls sword physics based on camera angular velocity.
/// Fast camera rotation = fast sword swing.
/// Attach this to the sword GameObject with a Rigidbody component.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SwordPhysicsController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Camera with CameraAngularVelocityTracker component")]
    public CameraAngularVelocityTracker cameraTracker;

    [Header("Sword Physics Settings")]
    [Tooltip("How strongly camera rotation affects sword (multiplier)")]
    public float swingForceMultiplier = 2.0f;

    [Tooltip("How strongly camera rotation creates torque on sword")]
    public float torqueMultiplier = 1.5f;

    [Tooltip("Maximum angular velocity for the sword (prevents crazy spinning)")]
    public float maxAngularVelocity = 50f;

    [Tooltip("Drag applied when no camera movement detected")]
    public float restDrag = 5f;

    [Tooltip("Drag applied during active swinging")]
    public float swingDrag = 0.5f;

    [Tooltip("Minimum camera angular speed to trigger swing")]
    public float swingThreshold = 10f;

    [Header("Sword Positioning")]
    [Tooltip("Local position offset from camera")]
    public Vector3 swordOffset = new Vector3(0.5f, -0.3f, 0.8f);

    [Tooltip("How quickly sword follows camera position")]
    public float positionFollowSpeed = 10f;

    // Components
    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Auto-find camera tracker if not assigned
        if (cameraTracker == null)
        {
            cameraTracker = Camera.main?.GetComponent<CameraAngularVelocityTracker>();
            if (cameraTracker == null)
            {
                Debug.LogError("SwordPhysicsController: No CameraAngularVelocityTracker found! Please assign or add to main camera.");
            }
        }

        if (cameraTracker != null)
        {
            cameraTransform = cameraTracker.transform;
        }

        // Configure rigidbody
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.drag = restDrag;
    }

    void FixedUpdate()
    {
        if (cameraTracker == null || cameraTransform == null)
            return;

        // Get camera angular velocity
        Vector3 cameraAngularVelocity = cameraTracker.AngularVelocity;
        float cameraAngularSpeed = cameraTracker.AngularSpeed;

        // Check if camera is rotating fast enough to trigger swing
        bool isSwinging = cameraAngularSpeed > swingThreshold;

        // Adjust drag based on whether we're swinging
        rb.drag = isSwinging ? swingDrag : restDrag;

        if (isSwinging)
        {
            // Apply torque based on camera rotation
            // Convert angular velocity from degrees/sec to radians/sec and apply
            Vector3 torque = cameraAngularVelocity * torqueMultiplier * Mathf.Deg2Rad;
            rb.AddTorque(torque, ForceMode.Force);

            // Apply force in the direction of camera rotation
            // This creates a "sweeping" motion
            Vector3 swingDirection = Vector3.Cross(cameraAngularVelocity.normalized, transform.forward);
            Vector3 swingForce = swingDirection * cameraAngularSpeed * swingForceMultiplier;
            rb.AddForce(swingForce, ForceMode.Force);
        }

        // Position sword relative to camera
        Vector3 targetPosition = cameraTransform.position + cameraTransform.TransformDirection(swordOffset);
        Vector3 positionDelta = targetPosition - transform.position;

        // Use velocity to smoothly move toward target position
        Vector3 targetVelocity = positionDelta * positionFollowSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * positionFollowSpeed);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize sword position offset in editor
        if (cameraTransform != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 targetPos = cameraTransform.position + cameraTransform.TransformDirection(swordOffset);
            Gizmos.DrawWireSphere(targetPos, 0.1f);
            Gizmos.DrawLine(cameraTransform.position, targetPos);
        }
    }
}
