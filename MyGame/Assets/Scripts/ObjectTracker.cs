using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages tracking of all TrackableObject instances and detects when they fall off the platform.
/// Tracks objects based on Y threshold and circular platform bounds.
/// </summary>
public class ObjectTracker : MonoBehaviour
{
    [Header("Platform Settings")]
    [Tooltip("Center point of the circular platform")]
    public Vector3 platformCenter = Vector3.zero;

    [Tooltip("Radius of the circular platform")]
    public float platformRadius = 10f;

    [Tooltip("Y position below which objects are considered fallen")]
    public float fallThreshold = -5f;

    [Header("Tracking Settings")]
    [Tooltip("How often to check object positions (in seconds, 0 = every frame)")]
    public float checkInterval = 0f;

    [Tooltip("Auto-detect platform center and radius from GameObject with 'Platform' tag")]
    public bool autoDetectPlatform = true;

    [Header("Events")]
    public UnityEvent<TrackableObject> onObjectFell;
    public UnityEvent<int> onObjectCountChanged;

    // Singleton instance
    private static ObjectTracker instance;
    public static ObjectTracker Instance => instance;

    // Tracking data
    private List<TrackableObject> trackedObjects = new List<TrackableObject>();
    private HashSet<TrackableObject> objectsOnPlatform = new HashSet<TrackableObject>();
    private HashSet<TrackableObject> objectsOffPlatform = new HashSet<TrackableObject>();
    private float lastCheckTime = 0f;

    /// <summary>
    /// Gets all objects currently on the platform
    /// </summary>
    public IReadOnlyCollection<TrackableObject> ObjectsOnPlatform => objectsOnPlatform;

    /// <summary>
    /// Gets all objects that have fallen off the platform
    /// </summary>
    public IReadOnlyCollection<TrackableObject> ObjectsOffPlatform => objectsOffPlatform;

    /// <summary>
    /// Gets the count of objects currently on the platform
    /// </summary>
    public int OnPlatformCount => objectsOnPlatform.Count;

    /// <summary>
    /// Gets the count of objects that have fallen off
    /// </summary>
    public int OffPlatformCount => objectsOffPlatform.Count;

    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Auto-detect platform settings if enabled
        if (autoDetectPlatform)
        {
            DetectPlatformSettings();
        }
    }

    private void Start()
    {
        // Find all trackable objects in the scene
        RefreshTrackedObjects();
    }

    private void Update()
    {
        // Check if it's time to update tracking
        if (checkInterval > 0f)
        {
            if (Time.time - lastCheckTime < checkInterval)
                return;
            lastCheckTime = Time.time;
        }

        UpdateTracking();
    }

    /// <summary>
    /// Manually refresh the list of tracked objects
    /// </summary>
    public void RefreshTrackedObjects()
    {
        trackedObjects.Clear();
        trackedObjects.AddRange(FindObjectsOfType<TrackableObject>());

        // Initialize all objects as on-platform
        objectsOnPlatform.Clear();
        objectsOffPlatform.Clear();

        foreach (var obj in trackedObjects)
        {
            if (obj.isTracked)
            {
                objectsOnPlatform.Add(obj);
                obj.IsOnPlatform = true;
            }
        }

        onObjectCountChanged?.Invoke(objectsOnPlatform.Count);
    }

    /// <summary>
    /// Register a new trackable object at runtime
    /// </summary>
    public void RegisterObject(TrackableObject obj)
    {
        if (!trackedObjects.Contains(obj))
        {
            trackedObjects.Add(obj);
            objectsOnPlatform.Add(obj);
            obj.IsOnPlatform = true;
            onObjectCountChanged?.Invoke(objectsOnPlatform.Count);
        }
    }

    /// <summary>
    /// Unregister a trackable object
    /// </summary>
    public void UnregisterObject(TrackableObject obj)
    {
        if (trackedObjects.Contains(obj))
        {
            trackedObjects.Remove(obj);
            objectsOnPlatform.Remove(obj);
            objectsOffPlatform.Remove(obj);
            onObjectCountChanged?.Invoke(objectsOnPlatform.Count);
        }
    }

    private void UpdateTracking()
    {
        foreach (var obj in trackedObjects)
        {
            if (obj == null || !obj.isTracked)
                continue;

            bool isOnPlatform = CheckIfOnPlatform(obj.transform.position);
            bool wasOnPlatform = obj.IsOnPlatform;

            // Object just fell off
            if (wasOnPlatform && !isOnPlatform)
            {
                obj.IsOnPlatform = false;
                objectsOnPlatform.Remove(obj);
                objectsOffPlatform.Add(obj);

                onObjectFell?.Invoke(obj);
                onObjectCountChanged?.Invoke(objectsOnPlatform.Count);

                Debug.Log($"Object '{obj.objectId}' fell off the platform at position {obj.transform.position}");
            }
            // Object got back on platform (e.g., thrown back up)
            else if (!wasOnPlatform && isOnPlatform)
            {
                obj.IsOnPlatform = true;
                objectsOffPlatform.Remove(obj);
                objectsOnPlatform.Add(obj);
                onObjectCountChanged?.Invoke(objectsOnPlatform.Count);

                Debug.Log($"Object '{obj.objectId}' returned to the platform");
            }
        }
    }

    /// <summary>
    /// Check if a position is considered on the platform
    /// </summary>
    private bool CheckIfOnPlatform(Vector3 position)
    {
        // Check Y threshold - object fell below minimum height
        if (position.y < fallThreshold)
            return false;

        // Check circular bounds - object is outside platform radius
        Vector3 horizontalPosition = new Vector3(position.x, platformCenter.y, position.z);
        Vector3 horizontalCenter = new Vector3(platformCenter.x, platformCenter.y, platformCenter.z);
        float distanceFromCenter = Vector3.Distance(horizontalPosition, horizontalCenter);

        if (distanceFromCenter > platformRadius)
            return false;

        return true;
    }

    /// <summary>
    /// Attempt to auto-detect platform settings from a GameObject tagged 'Platform'
    /// </summary>
    private void DetectPlatformSettings()
    {
        GameObject platform = GameObject.FindGameObjectWithTag("Platform");
        if (platform != null)
        {
            // Use platform's position as center
            platformCenter = platform.transform.position;

            // Try to get radius from collider
            Collider platformCollider = platform.GetComponent<Collider>();
            if (platformCollider != null)
            {
                Bounds bounds = platformCollider.bounds;
                // Use the larger of width or depth as diameter, then divide by 2
                platformRadius = Mathf.Max(bounds.size.x, bounds.size.z) / 2f;

                // Set fall threshold slightly below platform
                fallThreshold = bounds.min.y - 2f;

                Debug.Log($"Auto-detected platform: center={platformCenter}, radius={platformRadius}, threshold={fallThreshold}");
            }
        }
    }

    /// <summary>
    /// Check if a specific position would be on the platform
    /// </summary>
    public bool IsPositionOnPlatform(Vector3 position)
    {
        return CheckIfOnPlatform(position);
    }

    /// <summary>
    /// Get all objects currently on the platform as a list
    /// </summary>
    public List<TrackableObject> GetObjectsOnPlatformList()
    {
        return new List<TrackableObject>(objectsOnPlatform);
    }

    /// <summary>
    /// Get all objects that have fallen off as a list
    /// </summary>
    public List<TrackableObject> GetObjectsOffPlatformList()
    {
        return new List<TrackableObject>(objectsOffPlatform);
    }

    // Debug visualization
    private void OnDrawGizmos()
    {
        // Draw platform bounds
        Gizmos.color = Color.green;
        DrawCircle(platformCenter, platformRadius, 32);

        // Draw fall threshold plane
        Gizmos.color = Color.red;
        DrawCircle(new Vector3(platformCenter.x, fallThreshold, platformCenter.z), platformRadius * 1.2f, 32);
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}
