# Camera-Based Sword Swinging System

This system implements physics-based sword swinging controlled by camera rotation. Fast camera movements result in fast sword swings.

## Components

### 1. CameraAngularVelocityTracker
Tracks the camera's rotational velocity in degrees per second.

**Setup:**
- Attach to Main Camera GameObject
- Configure smoothing (0-1, default 0.3)
- Lower smoothing = smoother but slower response
- Higher smoothing = more responsive but jittery

**Public API:**
- `Vector3 AngularVelocity` - Current angular velocity
- `float AngularSpeed` - Magnitude of angular velocity
- `GetAngularVelocityInDirection(Vector3)` - Velocity in specific direction

### 2. SwordPhysicsController
Controls sword physics based on camera angular velocity.

**Setup:**
1. Create sword GameObject with:
   - Mesh (your sword model)
   - Rigidbody component
   - Collider (for physics interactions)

2. Attach `SwordPhysicsController` script to sword GameObject

3. Configure in Inspector:
   - **Camera Tracker**: Auto-finds Main Camera tracker, or assign manually
   - **Swing Force Multiplier** (default: 2.0): How strongly camera rotation affects sword
   - **Torque Multiplier** (default: 1.5): Rotational force applied to sword
   - **Max Angular Velocity** (default: 50): Prevents over-spinning
   - **Swing Threshold** (default: 10): Minimum camera speed to trigger swing
   - **Sword Offset** (default: 0.5, -0.3, 0.8): Position relative to camera
   - **Position Follow Speed** (default: 10): How quickly sword follows camera

## How It Works

1. **Camera Tracking**: CameraAngularVelocityTracker calculates how fast the camera rotates each frame

2. **Threshold Detection**: When camera angular speed exceeds `swingThreshold`, swing mode activates

3. **Physics Application**:
   - **Torque**: Rotational force applied to spin the sword
   - **Swing Force**: Linear force in perpendicular direction creates sweeping motion
   - **Drag**: Lower drag during swings for momentum, higher at rest for stability

4. **Position Following**: Sword smoothly follows camera position with offset, allowing free swinging within bounds

## Tuning Tips

**For heavier, more realistic swords:**
- Increase Rigidbody mass
- Increase torque multiplier
- Decrease swing force multiplier

**For lighter, faster swords:**
- Decrease Rigidbody mass
- Decrease torque multiplier
- Increase swing force multiplier

**For more responsive swinging:**
- Lower swing threshold
- Decrease camera tracker smoothing
- Increase position follow speed

**For combat interactions:**
- Add collision detection to sword
- Adjust Rigidbody continuous collision detection
- Layer masks for specific hit targets

## Example Scene Setup

```
Scene Hierarchy:
├── Main Camera
│   └── CameraAngularVelocityTracker (attached)
└── Player Sword
    ├── Rigidbody
    ├── Box Collider (or Capsule)
    ├── Mesh Renderer (sword model)
    └── SwordPhysicsController (attached)
```

## Dependencies

- Unity Engine (tested with 2022.3+)
- Physics system enabled
- Input system for camera control

## Implementation Notes

- Angular velocity calculated from quaternion deltas
- Smoothing prevents jitter from frame-to-frame noise
- NaN protection for zero-rotation cases
- Gizmos show target sword position in editor
