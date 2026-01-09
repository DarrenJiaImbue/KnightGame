using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper script to set up the knight, sword, and test objects in the scene.
/// Can be run from Unity Editor: GameObject -> Setup Knight and Sword
/// </summary>
public class SceneSetupHelper : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/Setup Knight and Sword")]
    static void SetupScene()
    {
        // Create Knight (placeholder capsule)
        GameObject knight = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        knight.name = "Knight";
        knight.transform.position = new Vector3(0, 1, 0);

        // Add knight controller
        KnightController knightController = knight.AddComponent<KnightController>();

        // Create Sword (cube)
        GameObject sword = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sword.name = "Sword";
        sword.transform.SetParent(knight.transform);
        sword.transform.localPosition = new Vector3(0.5f, 0, 0.5f);
        sword.transform.localScale = new Vector3(0.1f, 0.8f, 0.1f); // Long thin blade
        sword.transform.localEulerAngles = new Vector3(0, 0, 45f);

        // Configure sword physics
        // Remove default collider
        Collider swordCollider = sword.GetComponent<Collider>();
        if (swordCollider != null)
        {
            DestroyImmediate(swordCollider);
        }

        // Add box collider as trigger
        BoxCollider boxCollider = sword.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        // Add rigidbody (kinematic)
        Rigidbody swordRb = sword.AddComponent<Rigidbody>();
        swordRb.isKinematic = true;
        swordRb.useGravity = false;

        // Add sword physics script
        sword.AddComponent<SwordPhysics>();

        // Link sword to knight controller
        knightController.sword = sword;

        // Create test physics objects
        for (int i = 0; i < 3; i++)
        {
            GameObject testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            testObject.name = $"TestObject_{i}";
            testObject.transform.position = new Vector3(2 + i * 1.5f, 1, 0);

            // Add rigidbody for physics
            Rigidbody rb = testObject.AddComponent<Rigidbody>();
            rb.mass = 1f;

            // Random color for visual distinction
            Renderer renderer = testObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(Random.value, Random.value, Random.value);
                renderer.material = mat;
            }
        }

        Debug.Log("Scene setup complete! Knight, sword, and test objects created.");

        // Select the knight in hierarchy
        Selection.activeGameObject = knight;
    }

    [MenuItem("GameObject/Create Test Physics Object")]
    static void CreateTestObject()
    {
        GameObject testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        testObject.name = "TestObject";
        testObject.transform.position = new Vector3(2, 1, 0);

        // Add rigidbody
        Rigidbody rb = testObject.AddComponent<Rigidbody>();
        rb.mass = 1f;

        // Random color
        Renderer renderer = testObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(Random.value, Random.value, Random.value);
            renderer.material = mat;
        }

        Selection.activeGameObject = testObject;
        Debug.Log("Test object created!");
    }
#endif
}
