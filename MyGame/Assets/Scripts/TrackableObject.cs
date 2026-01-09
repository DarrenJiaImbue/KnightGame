using UnityEngine;

/// <summary>
/// Component for objects that need to be tracked for win condition.
/// Detects when object falls off platform and notifies GameManager.
/// </summary>
public class TrackableObject : MonoBehaviour
{
    [Header("Fall Detection")]
    [Tooltip("Y position below which object is considered off platform")]
    [SerializeField] private float fallThreshold = -5f;

    private bool isOnPlatform = true;

    public bool IsOnPlatform => isOnPlatform;

    private void Start()
    {
        // Register with GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterTrackableObject(this);
        }
        else
        {
            Debug.LogWarning("GameManager not found. TrackableObject will not be tracked.");
        }
    }

    private void Update()
    {
        CheckIfFallen();
    }

    private void CheckIfFallen()
    {
        // If object falls below threshold, mark as off platform
        if (isOnPlatform && transform.position.y < fallThreshold)
        {
            isOnPlatform = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnObjectFellOff(this);
            }

            Debug.Log($"{gameObject.name} fell off platform at Y={transform.position.y}");
        }
    }

    private void OnDestroy()
    {
        // Unregister when destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterTrackableObject(this);
        }
    }

    /// <summary>
    /// Manually reset object to on-platform state (useful for resets)
    /// </summary>
    public void ResetToOnPlatform()
    {
        isOnPlatform = true;
    }
}
