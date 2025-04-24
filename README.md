# Player_Movement_Script
C# Script for use with Unity Engine for player movement for a space shooter game.

This Unity C# script, PlayerTouchMovement, manages a 2D player's movement and rotation based on mouse or touch input. It allows the player character to either move toward a clicked or tapped point or be dragged directly when interacting with the player itself. The movement logic supports both desktop (mouse) and mobile (touch) input, automatically adapting depending on the platform.

When the player clicks or taps outside the player’s collider, the character moves toward that location at a specified move speed. If the player clicks or taps on the player, it enters a dragging state, allowing direct manipulation of the player’s position, following the input point. During dragging, the script also enables the player's Shooter component’s isFiring property, allowing the player to shoot while being dragged.

In addition to movement, the script handles rotation, smoothly tilting the player left or right depending on movement direction, using adjustable rotation amount and rotation speed values. The script includes helper methods for detecting player clicks (via 2D collider overlap), updating position, and smoothly rotating the player. This design ensures responsive controls for both point-to-move and drag-to-move gameplay styles, supporting shooting during dragging.
