using UnityEngine;

public class HitResponder : MonoBehaviour
{
    public float hitForceMultiplier = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning($"HitResponder on {gameObject.name} requires a Rigidbody component!");
        }
    }

    // Called when hit by sword or other object
    public void OnHit(Vector3 hitPoint, Vector3 hitDirection, float force)
    {
        if (rb != null)
        {
            // Apply force at the hit point
            rb.AddForceAtPosition(hitDirection * force * hitForceMultiplier, hitPoint, ForceMode.Impulse);

            // Optional: Change color briefly to indicate hit
            StartCoroutine(FlashOnHit());
        }
    }

    // Visual feedback when hit
    private System.Collections.IEnumerator FlashOnHit()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;
            Color originalColor = mat.color;
            mat.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            mat.color = originalColor;
        }
    }

    // Alternative: Detect collision with sword directly
    void OnCollisionEnter(Collision collision)
    {
        // If the colliding object is tagged as "Sword"
        if (collision.gameObject.CompareTag("Sword"))
        {
            // Calculate hit direction from collision
            Vector3 hitDirection = collision.contacts[0].normal * -1;
            Vector3 hitPoint = collision.contacts[0].point;

            // Get sword's velocity magnitude as force
            float force = collision.relativeVelocity.magnitude;

            OnHit(hitPoint, hitDirection, force);
        }
    }
}
