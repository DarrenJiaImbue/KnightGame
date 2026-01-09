using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    void Start()
    {
        CreatePlatform();
        CreatePhysicsObjects();
        SetupPlayer();
    }

    void SetupPlayer()
    {
        // Find the main camera and add sword controller to it
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera != null)
        {
            SwordController swordController = camera.GetComponent<SwordController>();
            if (swordController == null)
            {
                camera.AddComponent<SwordController>();
            }
        }
    }

    void CreatePlatform()
    {
        // Create a platform (flat cube)
        GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.name = "Platform";
        platform.transform.position = new Vector3(0, -0.5f, 0);
        platform.transform.localScale = new Vector3(10, 1, 10);

        // Platform should be static (no physics)
        Rigidbody platformRb = platform.GetComponent<Rigidbody>();
        if (platformRb != null)
        {
            Destroy(platformRb);
        }
    }

    void CreatePhysicsObjects()
    {
        // Create cubes
        CreatePhysicsObject(PrimitiveType.Cube, new Vector3(-3, 2, 0), "Cube1");
        CreatePhysicsObject(PrimitiveType.Cube, new Vector3(-1, 2, 0), "Cube2");
        CreatePhysicsObject(PrimitiveType.Cube, new Vector3(1, 2, 0), "Cube3");

        // Create spheres
        CreatePhysicsObject(PrimitiveType.Sphere, new Vector3(3, 2, 0), "Sphere1");
        CreatePhysicsObject(PrimitiveType.Sphere, new Vector3(-2, 3, 2), "Sphere2");

        // Create a cylinder for variety
        CreatePhysicsObject(PrimitiveType.Cylinder, new Vector3(2, 2, 2), "Cylinder1");
    }

    void CreatePhysicsObject(PrimitiveType type, Vector3 position, string name)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.name = name;
        obj.transform.position = position;

        // Add Rigidbody for physics
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
        }

        // Set physics properties
        rb.mass = 1.0f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;

        // Collider is automatically added by CreatePrimitive

        // Add the HitResponder script so objects respond to sword hits
        obj.AddComponent<HitResponder>();
    }
}
