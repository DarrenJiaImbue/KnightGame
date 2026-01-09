using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages game state and win condition detection.
/// Tracks all objects and triggers win event when all have fallen off platform.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;
    public static GameManager Instance => instance;

    [Header("Win Condition")]
    [Tooltip("Minimum number of objects required to trigger win condition")]
    [SerializeField] private int minimumObjectsToTrack = 1;

    [Tooltip("Event triggered when win condition is met")]
    public UnityEvent OnWinConditionMet;

    // Tracked objects
    private List<TrackableObject> trackedObjects = new List<TrackableObject>();
    private bool hasWon = false;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize event if null
        if (OnWinConditionMet == null)
        {
            OnWinConditionMet = new UnityEvent();
        }
    }

    /// <summary>
    /// Register a trackable object with the game manager
    /// </summary>
    public void RegisterTrackableObject(TrackableObject obj)
    {
        if (!trackedObjects.Contains(obj))
        {
            trackedObjects.Add(obj);
            Debug.Log($"Registered {obj.gameObject.name}. Total tracked objects: {trackedObjects.Count}");
        }
    }

    /// <summary>
    /// Unregister a trackable object (called on destroy)
    /// </summary>
    public void UnregisterTrackableObject(TrackableObject obj)
    {
        trackedObjects.Remove(obj);
        Debug.Log($"Unregistered {obj.gameObject.name}. Total tracked objects: {trackedObjects.Count}");
    }

    /// <summary>
    /// Called when an object falls off the platform
    /// </summary>
    public void OnObjectFellOff(TrackableObject obj)
    {
        Debug.Log($"{obj.gameObject.name} fell off platform");
        CheckWinCondition();
    }

    /// <summary>
    /// Check if win condition is met (all tracked objects are off platform)
    /// </summary>
    private void CheckWinCondition()
    {
        // Don't check if already won
        if (hasWon)
            return;

        // Need at least minimum objects to track
        if (trackedObjects.Count < minimumObjectsToTrack)
        {
            Debug.Log($"Not enough objects to check win condition. Have {trackedObjects.Count}, need {minimumObjectsToTrack}");
            return;
        }

        // Check if all objects are off platform
        bool allObjectsOff = true;
        int objectsOffCount = 0;

        foreach (TrackableObject obj in trackedObjects)
        {
            if (obj == null)
                continue;

            if (obj.IsOnPlatform)
            {
                allObjectsOff = false;
            }
            else
            {
                objectsOffCount++;
            }
        }

        Debug.Log($"Win condition check: {objectsOffCount}/{trackedObjects.Count} objects off platform");

        // If all objects are off platform, trigger win
        if (allObjectsOff && trackedObjects.Count > 0)
        {
            TriggerWin();
        }
    }

    /// <summary>
    /// Trigger the win condition
    /// </summary>
    private void TriggerWin()
    {
        if (hasWon)
            return;

        hasWon = true;
        Debug.Log("🎉 WIN CONDITION MET! All objects knocked off platform!");

        // Invoke the win event
        OnWinConditionMet?.Invoke();
    }

    /// <summary>
    /// Reset the game state (for restart functionality)
    /// </summary>
    public void ResetGame()
    {
        hasWon = false;

        // Reset all tracked objects
        foreach (TrackableObject obj in trackedObjects)
        {
            if (obj != null)
            {
                obj.ResetToOnPlatform();
            }
        }

        Debug.Log("Game reset");
    }

    /// <summary>
    /// Get current game statistics
    /// </summary>
    public void GetGameStats(out int totalObjects, out int objectsOffPlatform)
    {
        totalObjects = trackedObjects.Count;
        objectsOffPlatform = 0;

        foreach (TrackableObject obj in trackedObjects)
        {
            if (obj != null && !obj.IsOnPlatform)
            {
                objectsOffPlatform++;
            }
        }
    }

    // Public properties for external access
    public bool HasWon => hasWon;
    public int TrackedObjectCount => trackedObjects.Count;
}
