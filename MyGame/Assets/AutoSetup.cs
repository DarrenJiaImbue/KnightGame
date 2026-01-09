using UnityEngine;

public class AutoSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnSceneLoaded()
    {
        // Check if setup has already been done
        GameObject existingSetup = GameObject.Find("GameManager");
        if (existingSetup != null)
        {
            return; // Already set up
        }

        // Create a game manager object
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<SceneSetup>();

        Debug.Log("Scene automatically set up with platform and physics objects!");
    }
}
