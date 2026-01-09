# Win State UI System

## Overview
The Win State UI system provides a complete interface for displaying victory conditions and allowing players to restart the game.

## Setup Instructions

### Quick Setup (Unity Editor)
1. Open your Unity project
2. In the Unity menu, go to `GameObject > UI > Win State UI`
3. This will automatically create:
   - Canvas (if not already present)
   - Win Panel with semi-transparent black background
   - Win message text
   - Restart button
   - WinStateUI component attached to the Canvas

### Manual Setup (Alternative)
If you prefer to set up manually:
1. Create a Canvas in your scene
2. Add the `WinStateUI` component to the Canvas GameObject
3. Create a Panel as a child of Canvas and assign it to the `Win Panel` field
4. Add a Text component for the win message and assign it to `Win Message Text`
5. Add a Button for restart and assign it to `Restart Button`

## Features

### Core Functionality
- **Show Win State**: Displays the win UI and pauses the game
- **Hide Win State**: Hides the win UI and resumes the game
- **Restart Game**: Reloads the current scene with proper cleanup

### Configurable Settings
- **Win Message**: Customizable victory text (default: "Victory! All objects cleared!")
- **Pause Game On Win**: Toggle whether to pause game when win state shows (default: true)
- **Test Mode**: Enable/disable test trigger (default: enabled)
- **Test Win Key**: Key to trigger win state in test mode (default: W)

## Usage

### From Code (Win Detection System)
When the win condition is detected (e.g., all objects knocked off platform), call:

```csharp
WinStateUI winUI = FindObjectOfType<WinStateUI>();
if (winUI != null)
{
    winUI.ShowWinState();
}
```

### Test Mode
While in Play mode, press the **W** key to trigger the win state for testing purposes.
This is useful for:
- Testing UI layout and appearance
- Verifying restart functionality
- Integration testing before win detection (ga-3zz) is implemented

To disable test mode, uncheck "Enable Test Mode" in the WinStateUI inspector.

## Integration Points

### Win Condition Detection (ga-3zz)
Once the win condition detection system is implemented, it should:
1. Monitor tracked objects (via ga-8cl: object tracking system)
2. Detect when all objects are off the platform
3. Call `winUI.ShowWinState()` to trigger the victory UI

Example integration:
```csharp
public class WinConditionDetector : MonoBehaviour
{
    private WinStateUI winUI;

    void Start()
    {
        winUI = FindObjectOfType<WinStateUI>();
    }

    void CheckWinCondition()
    {
        if (AllObjectsOffPlatform())
        {
            winUI.ShowWinState();
        }
    }
}
```

## Technical Details

### Time Scale Management
- When win state shows: `Time.timeScale = 0f` (game paused)
- When win state hides or restarts: `Time.timeScale = 1f` (game resumed)
- Proper cleanup in `OnDestroy()` ensures time scale is always reset

### Scene Management
- Uses `SceneManager.LoadScene()` to restart
- Reloads the current active scene
- All GameObjects are properly destroyed and recreated

## Files
- `WinStateUI.cs`: Main component script
- `Editor/WinStateUISetup.cs`: Editor tool for automatic UI creation

## Dependencies
- UnityEngine.UI
- UnityEngine.SceneManagement

## Related Beads
- ga-uaz: Create win state UI (this implementation)
- ga-3zz: Add win condition detection (integration point)
- ga-8cl: Implement object tracking system (data source for win detection)
