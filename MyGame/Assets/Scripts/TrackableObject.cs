using UnityEngine;

/// <summary>
/// Component that marks a GameObject as trackable by the ObjectTracker system.
/// Attach this to any object you want to monitor for platform falls.
/// </summary>
public class TrackableObject : MonoBehaviour
{
    [Tooltip("Unique identifier for this object (optional)")]
    public string objectId;

    [Tooltip("Whether this object is currently being tracked")]
    public bool isTracked = true;

    private bool wasOnPlatform = true;

    /// <summary>
    /// Whether this object is currently on the platform
    /// </summary>
    public bool IsOnPlatform
    {
        get { return wasOnPlatform; }
        internal set { wasOnPlatform = value; }
    }

    private void Awake()
    {
        // Auto-generate ID if not set
        if (string.IsNullOrEmpty(objectId))
        {
            objectId = gameObject.name + "_" + GetInstanceID();
        }
    }
}
