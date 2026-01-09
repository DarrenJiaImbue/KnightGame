using UnityEngine;

/// <summary>
/// Handles sword physics interactions - applies force to objects on collision.
/// Attach to sword GameObject with Rigidbody and Collider components.
/// </summary>
public class SwordPhysics : MonoBehaviour
{
    [Header("Physics Settings")]
    [Tooltip("Base force multiplier for sword strikes")]
    [SerializeField] private float strikeForce = 10f;

    [Tooltip("Minimum velocity required to register a hit")]
    [SerializeField] private float minimumVelocity = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private Rigidbody swordRigidbody;

    void Start()
    {
        // Get or add Rigidbody component
        swordRigidbody = GetComponent<Rigidbody>();
        if (swordRigidbody == null)
        {
            swordRigidbody = gameObject.AddComponent<Rigidbody>();
            Debug.LogWarning("SwordPhysics: Rigidbody not found, added automatically");
        }

        // Ensure sword has kinematic rigidbody (controlled by animation/parent)
        swordRigidbody.isKinematic = true;

        // Get or add Collider
        if (GetComponent<Collider>() == null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true; // Use trigger to detect hits without physics interference
            Debug.LogWarning("SwordPhysics: Collider not found, added BoxCollider as trigger");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Calculate sword velocity magnitude
        Vector3 velocity = swordRigidbody.velocity;
        float speed = velocity.magnitude;

        // Only apply force if moving fast enough
        if (speed < minimumVelocity)
        {
            return;
        }

        // Try to apply force to the hit object
        Rigidbody hitRigidbody = other.GetComponent<Rigidbody>();
        if (hitRigidbody != null && !hitRigidbody.isKinematic)
        {
            // Calculate force direction (from sword velocity)
            Vector3 forceDirection = velocity.normalized;

            // Apply force scaled by sword speed
            float appliedForce = strikeForce * speed;
            hitRigidbody.AddForce(forceDirection * appliedForce, ForceMode.Impulse);

            if (showDebugLogs)
            {
                Debug.Log($"SwordPhysics: Hit {other.name} with force {appliedForce:F1} at speed {speed:F2}");
            }
        }
    }

    /// <summary>
    /// Calculate and visualize sword velocity for debugging
    /// </summary>
    void Update()
    {
        if (showDebugLogs && swordRigidbody != null)
        {
            // Draw velocity vector in scene view
            Debug.DrawRay(transform.position, swordRigidbody.velocity, Color.red);
        }
    }
}
