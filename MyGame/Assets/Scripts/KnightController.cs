using UnityEngine;

/// <summary>
/// Manages the knight character and sword positioning.
/// The sword is parented to the knight and positioned in front.
/// </summary>
public class KnightController : MonoBehaviour
{
    [Header("Sword Settings")]
    [Tooltip("Reference to the sword GameObject")]
    public GameObject sword;

    [Tooltip("Position offset of sword relative to knight")]
    [SerializeField] private Vector3 swordOffset = new Vector3(0.5f, 0f, 0.5f);

    [Tooltip("Rotation offset of sword")]
    [SerializeField] private Vector3 swordRotation = new Vector3(0f, 0f, 45f);

    void Start()
    {
        // Ensure sword is parented to knight
        if (sword != null && sword.transform.parent != transform)
        {
            sword.transform.SetParent(transform);
            sword.transform.localPosition = swordOffset;
            sword.transform.localEulerAngles = swordRotation;
        }
    }

    void Update()
    {
        // Keep sword positioned correctly (in case it gets moved)
        if (sword != null)
        {
            sword.transform.localPosition = swordOffset;
        }
    }

    /// <summary>
    /// Editor helper to visualize sword position
    /// </summary>
    void OnDrawGizmos()
    {
        if (sword != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sword.transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, sword.transform.position);
        }
    }
}
