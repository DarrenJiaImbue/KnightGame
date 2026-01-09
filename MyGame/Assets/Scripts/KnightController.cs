using UnityEngine;

/// <summary>
/// Controls the knight character. This is a placeholder controller
/// that will be extended with movement and other functionality.
/// </summary>
public class KnightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform swordTransform;

    [Header("Knight Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        // Initialization logic
    }

    private void Start()
    {
        // Setup sword reference if not assigned
        if (swordTransform == null)
        {
            // Look for sword in children
            Transform sword = transform.Find("Sword");
            if (sword != null)
            {
                swordTransform = sword;
            }
        }
    }

    private void Update()
    {
        // Character update logic will be added here
    }

    /// <summary>
    /// Gets the sword transform attached to this knight
    /// </summary>
    public Transform GetSwordTransform()
    {
        return swordTransform;
    }
}
