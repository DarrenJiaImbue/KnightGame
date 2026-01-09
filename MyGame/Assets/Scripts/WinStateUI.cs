using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinStateUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text winMessageText;
    [SerializeField] private Button restartButton;

    [Header("Settings")]
    [SerializeField] private string winMessage = "Victory! All objects cleared!";
    [SerializeField] private bool pauseGameOnWin = true;

    [Header("Test Controls")]
    [SerializeField] private KeyCode testWinKey = KeyCode.W;
    [SerializeField] private bool enableTestMode = true;

    private void Start()
    {
        // Hide win panel at start
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        // Setup restart button
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        // Set win message
        if (winMessageText != null)
        {
            winMessageText.text = winMessage;
        }
    }

    private void Update()
    {
        // Test mode: Press W to trigger win state
        if (enableTestMode && Input.GetKeyDown(testWinKey))
        {
            ShowWinState();
        }
    }

    /// <summary>
    /// Shows the win state UI. Called by win condition detection system.
    /// </summary>
    public void ShowWinState()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);

            if (pauseGameOnWin)
            {
                Time.timeScale = 0f;
            }
        }
    }

    /// <summary>
    /// Hides the win state UI.
    /// </summary>
    public void HideWinState()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);

            if (pauseGameOnWin)
            {
                Time.timeScale = 1f;
            }
        }
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void RestartGame()
    {
        // Reset time scale before reloading
        Time.timeScale = 1f;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        // Ensure time scale is reset when destroyed
        Time.timeScale = 1f;
    }
}
