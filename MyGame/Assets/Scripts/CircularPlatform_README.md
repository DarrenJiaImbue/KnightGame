# Circular Platform Arena

## Overview
A procedurally generated circular platform with optional edge rails for arena-based gameplay.

## Features
- **Circular mesh generation** with customizable radius and segments
- **Proper collision detection** using MeshCollider
- **Optional edge rails** to prevent players from falling off
- **Fully customizable** through Inspector properties
- **Visually distinct** with support for custom materials

## Setup Instructions

### Basic Setup
1. Create an empty GameObject in your scene (GameObject → Create Empty)
2. Rename it to "CircularPlatform"
3. Add the `CircularPlatform` component to it
4. The platform will generate automatically on Start()

### Customization Options

#### Platform Settings
- **Radius**: Size of the circular platform (default: 10 units)
- **Segments**: Number of segments for circle smoothness (default: 64)
  - Higher values = smoother circle, more vertices
  - Lower values = more angular, better performance
- **Height**: Thickness of the platform (default: 0.5 units)

#### Edge Settings
- **Add Edge Rail**: Enable/disable protective edge barrier
- **Rail Height**: Height of the edge barrier (default: 1 unit)
- **Rail Thickness**: Width of the edge rail (default: 0.2 units)

#### Materials
- **Platform Material**: Assign a material for the main platform surface
- **Rail Material**: Assign a material for the edge rails (if enabled)

## Example Configuration

### Small Arena
```
Radius: 8
Segments: 48
Height: 0.3
Add Edge Rail: true
Rail Height: 0.8
```

### Large Arena
```
Radius: 20
Segments: 96
Height: 0.8
Add Edge Rail: true
Rail Height: 1.5
```

### Performance Mode
```
Radius: 10
Segments: 32
Height: 0.5
Add Edge Rail: false
```

## Technical Details

### Collision
- Uses non-convex MeshCollider for accurate collision detection
- Both platform and rails have collision enabled
- Suitable for character controllers and rigidbody physics

### Mesh Generation
- Generates vertices procedurally at runtime
- Includes proper normals, UVs, and tangents
- Optimized triangle generation for efficient rendering

### Runtime Editing
- Values can be adjusted in the Inspector during Play mode
- Changes trigger automatic mesh regeneration via OnValidate()

## Usage Tips

1. **Materials**: Create distinct materials for platform and rails to improve visual clarity
2. **Lighting**: Platform responds well to baked and realtime lighting
3. **Shadows**: Enable shadow casting for better depth perception
4. **Performance**: Reduce segments count if targeting low-end devices

## Integration

This platform works well with:
- Character controllers
- Physics-based gameplay
- Arena combat systems
- Object tracking systems

Position it at world origin (0, 0, 0) or adjust as needed for your scene layout.
