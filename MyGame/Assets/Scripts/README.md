# Sword Physics System

## Overview
This system implements sword physics interactions for the knight character. The sword has collider and rigidbody components that detect hits and apply force to objects.

## Components

### SwordPhysics.cs
Handles sword collision detection and force application.
- **Rigidbody**: Kinematic (controlled by parent/animation)
- **Collider**: Trigger-based to detect hits
- **Force Application**: Applies impulse force based on sword velocity
- **Parameters**:
  - `strikeForce`: Base force multiplier (default: 10)
  - `minimumVelocity`: Minimum speed to register hit (default: 0.5)

### KnightController.cs
Manages knight character and sword positioning.
- Keeps sword parented to knight
- Maintains sword offset position
- Provides visual debugging with gizmos

### SceneSetupHelper.cs
Editor utility for quick scene setup.
- Creates knight (capsule placeholder)
- Creates sword (cube) with physics
- Creates test physics objects
- **Usage**: In Unity Editor, go to `GameObject -> Setup Knight and Sword`

## Setup Instructions

### Method 1: Automated Setup (Recommended)
1. Open Unity Editor
2. Open `SampleScene`
3. Go to menu: `GameObject -> Setup Knight and Sword`
4. Scene will be populated with knight, sword, and test objects

### Method 2: Manual Setup
1. Create Knight:
   - Create Capsule GameObject
   - Add `KnightController` script
   - Position at (0, 1, 0)

2. Create Sword:
   - Create Cube GameObject
   - Scale: (0.1, 0.8, 0.1)
   - Parent to Knight
   - Local Position: (0.5, 0, 0.5)
   - Local Rotation: (0, 0, 45)
   - Add `BoxCollider` (set as trigger)
   - Add `Rigidbody` (set as kinematic)
   - Add `SwordPhysics` script
   - Assign to Knight's `sword` field

3. Create Test Objects:
   - Create Cube GameObjects
   - Add `Rigidbody` component
   - Position near knight

## Testing

### How to Test Sword Physics
1. Enter Play Mode
2. Move the knight (use WASD when movement is implemented)
3. Rotate camera quickly (sword should follow)
4. Sword hits should push test objects away
5. Check Console for hit debug logs

### Expected Behavior
- Fast sword movement = strong force
- Slow sword movement = weak/no force
- Objects should fly away from sword strikes
- Debug rays show sword velocity (red lines)

## Configuration

### Adjusting Strike Force
Select sword object, adjust `SwordPhysics` component:
- Increase `strikeForce` for more powerful hits
- Decrease `minimumVelocity` for more sensitive detection

### Adjusting Sword Position
Select knight object, adjust `KnightController` component:
- Modify `swordOffset` to reposition sword
- Modify `swordRotation` to change sword angle

## Integration with Other Systems

This implementation addresses:
- **ga-b27**: Sword physics interactions ✓
- **ga-4p4**: Knight character with sword model ✓ (partial - using placeholders)

Requires integration with:
- **ga-631**: Camera-based sword swinging (velocity from camera rotation)
- **ga-v2y**: WASD movement controller
- **ga-f34**: Camera control integration

## Notes
- Using placeholder models (Capsule for knight, Cube for sword)
- Final character models can be swapped in later
- Physics system is independent of visuals
- Rigidbody is kinematic - sword motion comes from animation/parent transform
