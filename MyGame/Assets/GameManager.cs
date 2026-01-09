using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // Add SceneSetup component if it doesn't exist
        SceneSetup sceneSetup = GetComponent<SceneSetup>();
        if (sceneSetup == null)
        {
            gameObject.AddComponent<SceneSetup>();
        }
    }

    void Update()
    {
        // Press Escape to quit (useful for testing)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
