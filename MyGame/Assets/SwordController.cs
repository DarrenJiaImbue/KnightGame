using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float swingForce = 20f;
    public float swingSpeed = 5f;

    private GameObject sword;
    private bool isSwinging = false;
    private float swingProgress = 0f;
    private Vector3 startRotation = new Vector3(-90, 0, 0);
    private Vector3 endRotation = new Vector3(0, 0, 0);

    void Start()
    {
        CreateSword();
    }

    void CreateSword()
    {
        // Create a simple sword (using a scaled cube)
        sword = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sword.name = "Sword";
        sword.tag = "Sword";
        sword.transform.parent = transform;
        sword.transform.localPosition = new Vector3(1, 0, 1);
        sword.transform.localScale = new Vector3(0.2f, 0.2f, 2f);
        sword.transform.localRotation = Quaternion.Euler(startRotation);

        // Add Rigidbody for physics-based collisions
        Rigidbody rb = sword.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Sword is controlled by animation, not physics

        // Ensure collider exists (CreatePrimitive adds BoxCollider automatically)
        BoxCollider collider = sword.GetComponent<BoxCollider>();
        collider.isTrigger = false;

        // Make sword a distinct color
        Renderer renderer = sword.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(0.7f, 0.7f, 0.8f); // Silver color
        }
    }

    void Update()
    {
        // Swing on mouse click or space bar
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!isSwinging)
            {
                StartSwing();
            }
        }

        // Update swing animation
        if (isSwinging)
        {
            UpdateSwing();
        }

        // Simple camera follow (optional)
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right * 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * 5f * Time.deltaTime;

        // Mouse look (simple version)
        float mouseX = Input.GetAxis("Mouse X") * 2f;
        transform.Rotate(0, mouseX, 0);
    }

    void StartSwing()
    {
        isSwinging = true;
        swingProgress = 0f;
    }

    void UpdateSwing()
    {
        swingProgress += Time.deltaTime * swingSpeed;

        if (swingProgress <= 1f)
        {
            // Swing forward
            sword.transform.localRotation = Quaternion.Euler(
                Vector3.Lerp(startRotation, endRotation, swingProgress)
            );
        }
        else if (swingProgress <= 2f)
        {
            // Swing back
            sword.transform.localRotation = Quaternion.Euler(
                Vector3.Lerp(endRotation, startRotation, swingProgress - 1f)
            );
        }
        else
        {
            // Swing complete
            isSwinging = false;
            sword.transform.localRotation = Quaternion.Euler(startRotation);
        }
    }
}
