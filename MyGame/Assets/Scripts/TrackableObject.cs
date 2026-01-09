using UnityEngine;

/// <summary>
/// Component for objects that need to be tracked for win condition.
/// Detects when object falls off platform and notifies GameManager.
/// </summary>
public class TrackableObject : MonoBehaviour
{
    [Header("Tracking Settings")]
    [Tooltip("Unique identifier for this object")]
    [SerializeField] private string _objectId = "";

    [Tooltip("Whether this object should be tracked")]
    [SerializeField] private bool _isTracked = true;

    [Header("Fall Detection")]
    [Tooltip("Y position below which object is considered off platform")]
    [SerializeField] private float fallThreshold = -5f;

    private bool _isOnPlatform = true;

    /// <summary>
    /// Unique identifier for this object. Auto-generated if not set.
    /// </summary>
    public string objectId
    {
        get
        {
            if (string.IsNullOrEmpty(_objectId))
            {
                _objectId = gameObject.name;
            }
            return _objectId;
        }
        set => _objectId = value;
    }

    /// <summary>
    /// Whether this object should be tracked by the ObjectTracker system.
    /// </summary>
    public bool isTracked
    {
        get => _isTracked;
        set => _isTracked = value;
    }

    /// <summary>
    /// Whether the object is currently on the platform. Can be set externally by tracking systems.
    /// </summary>
    public bool IsOnPlatform
    {
        get => _isOnPlatform;
        set => _isOnPlatform = value;
    }

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
        if (_isOnPlatform && transform.position.y < fallThreshold)
        {
            _isOnPlatform = false;

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
        _isOnPlatform = true;
    }
}
