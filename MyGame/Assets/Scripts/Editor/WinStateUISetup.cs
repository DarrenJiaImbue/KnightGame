using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class WinStateUISetup : EditorWindow
{
    [MenuItem("GameObject/UI/Win State UI")]
    public static void CreateWinStateUI()
    {
        // Create canvas if it doesn't exist
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvasObj, "Create Canvas");
        }

        // Create win panel
        GameObject winPanel = new GameObject("WinPanel");
        winPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = winPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage = winPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);

        // Create container for centered content
        GameObject contentContainer = new GameObject("ContentContainer");
        contentContainer.transform.SetParent(winPanel.transform, false);

        RectTransform containerRect = contentContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(400, 300);

        VerticalLayoutGroup layout = contentContainer.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 20;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        // Create win message text
        GameObject textObj = new GameObject("WinMessageText");
        textObj.transform.SetParent(contentContainer.transform, false);

        Text winText = textObj.AddComponent<Text>();
        winText.text = "Victory! All objects cleared!";
        winText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        winText.fontSize = 32;
        winText.alignment = TextAnchor.MiddleCenter;
        winText.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(350, 80);

        // Create restart button
        GameObject buttonObj = new GameObject("RestartButton");
        buttonObj.transform.SetParent(contentContainer.transform, false);

        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.7f, 0.2f, 1f);

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(200, 50);

        // Create button text
        GameObject buttonTextObj = new GameObject("Text");
        buttonTextObj.transform.SetParent(buttonObj.transform, false);

        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "Restart";
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 24;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;

        RectTransform buttonTextRect = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;

        // Add WinStateUI component
        GameObject winUIObj = canvas.gameObject;
        WinStateUI winStateUI = winUIObj.AddComponent<WinStateUI>();

        // Use reflection to set private fields
        var winPanelField = typeof(WinStateUI).GetField("winPanel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var winMessageField = typeof(WinStateUI).GetField("winMessageText",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var restartButtonField = typeof(WinStateUI).GetField("restartButton",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (winPanelField != null) winPanelField.SetValue(winStateUI, winPanel);
        if (winMessageField != null) winMessageField.SetValue(winStateUI, winText);
        if (restartButtonField != null) restartButtonField.SetValue(winStateUI, button);

        // Register undo
        Undo.RegisterCreatedObjectUndo(winPanel, "Create Win State UI");

        // Select the created UI
        Selection.activeGameObject = winPanel;

        Debug.Log("Win State UI created successfully! Test with 'W' key during play mode.");
    }
}
