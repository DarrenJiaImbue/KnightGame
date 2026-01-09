# First-Person Camera and Movement System

This folder contains the scripts for first-person camera control and player movement.

## Components

### FirstPersonCamera.cs
Handles mouse-based camera rotation with the following features:
- Vertical camera rotation (pitch) with clamping
- Horizontal player rotation (yaw)
- Cursor locking for immersive gameplay
- Configurable mouse sensitivity
- Provides forward/right direction for movement

### PlayerController.cs
Handles player movement with camera-relative directions:
- WASD movement in the direction the camera is facing
- Sprint functionality (Left Shift by default)
- Gravity simulation
- Requires CharacterController component

## Setup Instructions

### 1. Create Player GameObject
1. Create an empty GameObject named "Player"
2. Add a CharacterController component
3. Configure CharacterController:
   - Height: 2
   - Radius: 0.5
   - Center: (0, 1, 0)

### 2. Setup Camera
1. Make the Main Camera a child of the Player GameObject
2. Position the camera at (0, 1.6, 0) to simulate eye height
3. Add the FirstPersonCamera component to the Main Camera
4. Assign the Player GameObject to the "Player Body" field

### 3. Setup Player Controller
1. Add the PlayerController component to the Player GameObject
2. The script will automatically find the FirstPersonCamera component
3. Adjust settings:
   - Move Speed: 5 (default walking speed)
   - Sprint Multiplier: 1.5 (sprint speed multiplier)
   - Gravity: -9.81 (default gravity)

### 4. Input System
The Input System Actions are already configured in `InputSystem_Actions.inputactions`:
- **Move**: WASD or Arrow Keys
- **Look**: Mouse movement
- **Sprint**: Left Shift

## Controls

- **W/A/S/D**: Move forward/left/backward/right relative to camera direction
- **Mouse**: Look around (camera follows mouse movement)
- **Left Shift**: Sprint (hold to move faster)

## How It Works

1. **FirstPersonCamera** reads mouse input via the Input System
2. The camera rotates vertically (up/down)
3. The player body rotates horizontally (left/right)
4. **PlayerController** gets the camera's forward direction
5. Movement input (WASD) is calculated relative to where the camera is looking
6. The player moves in the direction they're facing

## Customization

### Mouse Sensitivity
Adjust the `Mouse Sensitivity` field in the FirstPersonCamera component (default: 100)

### Vertical Look Clamp
Control how far the player can look up and down:
- Vertical Clamp Min: -90 (straight down)
- Vertical Clamp Max: 90 (straight up)

### Movement Speed
Adjust movement parameters in the PlayerController component:
- Move Speed: Base walking speed
- Sprint Multiplier: How much faster sprinting is
- Gravity: Strength of gravity effect
