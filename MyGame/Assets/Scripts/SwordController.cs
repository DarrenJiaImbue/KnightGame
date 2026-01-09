using UnityEngine;

/// <summary>
/// Controls the sword positioning and keeps it in front of the player.
/// The sword is parented to the player and positioned at a fixed offset.
/// </summary>
public class SwordController : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0.5f, 0f, 0.5f);
    [SerializeField] private Vector3 rotationOffset = new Vector3(45f, 0f, 0f);

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        // If player transform not set, try to find it
        if (playerTransform == null && transform.parent != null)
        {
            playerTransform = transform.parent;
        }

        // Set initial position relative to player
        UpdateSwordPosition();
    }

    private void LateUpdate()
    {
        // Keep sword positioned correctly relative to player
        UpdateSwordPosition();
    }

    /// <summary>
    /// Updates the sword's local position and rotation
    /// </summary>
    private void UpdateSwordPosition()
    {
        // Set local position offset
        transform.localPosition = positionOffset;

        // Set local rotation offset
        transform.localRotation = Quaternion.Euler(rotationOffset);
    }

    /// <summary>
    /// Sets a new position offset for the sword
    /// </summary>
    public void SetPositionOffset(Vector3 offset)
    {
        positionOffset = offset;
    }

    /// <summary>
    /// Sets a new rotation offset for the sword
    /// </summary>
    public void SetRotationOffset(Vector3 rotation)
    {
        rotationOffset = rotation;
    }
}
