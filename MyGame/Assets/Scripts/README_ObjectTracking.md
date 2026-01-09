# Object Tracking System

A flexible system for tracking objects on a circular platform and detecting when they fall off.

## Overview

The object tracking system consists of two main components:

1. **ObjectTracker** - Singleton manager that monitors all trackable objects
2. **TrackableObject** - Component attached to objects you want to track

## Setup

### 1. Add ObjectTracker to Scene

Create an empty GameObject in your scene and attach the `ObjectTracker` component to it.

**Inspector Settings:**
- **Platform Center**: Center point of your circular platform (default: 0,0,0)
- **Platform Radius**: Radius of the circular platform (default: 10)
- **Fall Threshold**: Y position below which objects are considered fallen (default: -5)
- **Check Interval**: How often to check positions (0 = every frame)
- **Auto Detect Platform**: Automatically detect settings from GameObject tagged "Platform"

### 2. Mark Objects as Trackable

Add the `TrackableObject` component to any GameObject you want to monitor:

```csharp
// Objects will be automatically discovered on Start()
// Or register manually at runtime:
ObjectTracker.Instance.RegisterObject(myTrackableObject);
```

### 3. Configure Platform (Optional)

For automatic detection, tag your platform GameObject with "Platform". The system will:
- Use the platform's position as center
- Calculate radius from the collider bounds
- Set fall threshold below the platform

## Usage

### Access the Tracker

```csharp
ObjectTracker tracker = ObjectTracker.Instance;
```

### Query State

```csharp
// Get counts
int onPlatform = tracker.OnPlatformCount;
int offPlatform = tracker.OffPlatformCount;

// Get collections
IReadOnlyCollection<TrackableObject> objectsOn = tracker.ObjectsOnPlatform;
IReadOnlyCollection<TrackableObject> objectsOff = tracker.ObjectsOffPlatform;

// Check a specific position
bool isOnPlatform = tracker.IsPositionOnPlatform(somePosition);
```

### Subscribe to Events

```csharp
void Start()
{
    ObjectTracker tracker = ObjectTracker.Instance;

    // Called when an object falls off
    tracker.onObjectFell.AddListener(OnObjectFell);

    // Called when the count of objects on platform changes
    tracker.onObjectCountChanged.AddListener(OnCountChanged);
}

void OnObjectFell(TrackableObject obj)
{
    Debug.Log($"Object {obj.objectId} fell off!");
    // Update game logic, play effects, etc.
}

void OnCountChanged(int newCount)
{
    Debug.Log($"{newCount} objects remaining on platform");
    // Update UI, check win conditions, etc.
}
```

### Runtime Management

```csharp
// Add objects at runtime
tracker.RegisterObject(newObject.GetComponent<TrackableObject>());

// Remove objects
tracker.UnregisterObject(objectToRemove);

// Refresh all tracked objects
tracker.RefreshTrackedObjects();
```

## Fall Detection

Objects are considered off-platform if either condition is true:

1. **Below Y Threshold**: `position.y < fallThreshold`
2. **Outside Radius**: Horizontal distance from center > `platformRadius`

## Example Implementation

See `ObjectTrackerExample.cs` for a complete working example.

## Integration with Game Logic

### Win Condition Detection

```csharp
void Update()
{
    // Check if all objects have fallen off
    if (ObjectTracker.Instance.OnPlatformCount == 0)
    {
        TriggerWinCondition();
    }
}
```

### Score Tracking

```csharp
void Start()
{
    ObjectTracker.Instance.onObjectFell.AddListener(obj => {
        score++;
        UpdateScoreUI();
    });
}
```

## Debug Visualization

When the ObjectTracker is selected in the Scene view, it draws:
- **Green circle**: Platform bounds at platform height
- **Red circle**: Fall threshold plane (slightly larger for visibility)

## Performance Notes

- Set `checkInterval > 0` to check positions less frequently if needed
- The system uses HashSet for O(1) lookups of object status
- Tracking is efficient for hundreds of objects

## Files

- `ObjectTracker.cs` - Main tracking manager
- `TrackableObject.cs` - Component for trackable objects
- `ObjectTrackerExample.cs` - Usage example
