using UnityEngine;

/// <summary>
/// Example script demonstrating how to use the ObjectTracker system.
/// This can be attached to any GameObject to see tracking events in action.
/// </summary>
public class ObjectTrackerExample : MonoBehaviour
{
    private void Start()
    {
        // Get the ObjectTracker instance
        ObjectTracker tracker = ObjectTracker.Instance;

        if (tracker != null)
        {
            // Subscribe to tracking events
            tracker.onObjectFell.AddListener(OnObjectFellOff);
            tracker.onObjectCountChanged.AddListener(OnObjectCountChanged);

            Debug.Log("ObjectTrackerExample: Subscribed to tracking events");
        }
        else
        {
            Debug.LogError("ObjectTrackerExample: No ObjectTracker found in scene!");
        }
    }

    private void OnObjectFellOff(TrackableObject obj)
    {
        Debug.Log($"Event: Object '{obj.objectId}' fell off the platform!");

        // Example: Could trigger game logic here
        // - Update score
        // - Check win conditions
        // - Play sound effect
        // - Show visual feedback
    }

    private void OnObjectCountChanged(int count)
    {
        Debug.Log($"Objects on platform: {count}");

        // Example: Could update UI here
        // - Update counter display
        // - Check if all objects have fallen
    }

    private void Update()
    {
        // Example: Query tracker state at any time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectTracker tracker = ObjectTracker.Instance;
            if (tracker != null)
            {
                Debug.Log($"Current state:");
                Debug.Log($"  Objects on platform: {tracker.OnPlatformCount}");
                Debug.Log($"  Objects off platform: {tracker.OffPlatformCount}");

                // Example: List all objects on platform
                foreach (var obj in tracker.ObjectsOnPlatform)
                {
                    Debug.Log($"    - {obj.objectId} at {obj.transform.position}");
                }
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up event subscriptions
        ObjectTracker tracker = ObjectTracker.Instance;
        if (tracker != null)
        {
            tracker.onObjectFell.RemoveListener(OnObjectFellOff);
            tracker.onObjectCountChanged.RemoveListener(OnObjectCountChanged);
        }
    }
}
