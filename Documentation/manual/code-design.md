# Code Design

Minimap is composed of a few components.

1. UI element to represent map on screen.
1. Bounds (world space of the minimap).
1. Scale (number of pixels per world unit).
1. Background texture.
1. Moving Icons.

## Minimap Implementations

The interface [IMinimap](xref:nickmaltbie.MinimapTools.Minimap.IMinimap)
represents the main responsibilities of a minimap. An important member of the IMinimap is the
[IMinimap.GetWorldBounds](xref:nickmaltbie.MinimapTools.Minimap.IMinimap.GetWorldBounds) function
which returns a [IMinimapShape](xref:nickmaltbie.MinimapTools.Minimap.Shape.IMinimapShape).
The minimap shape determines the space that the minimap
takes up in real world space and is used in the minimap's
function [IMinimap.GetMinimapPosition(Vector3)](xref:nickmaltbie.MinimapTools.Minimap.IMinimap.GetMinimapPosition(Vector3)) which converts from world space to minimap space.
World space is a [Vector3](xref:UnityEngine.Vector3) while minimap space is a
[Vector2](xref:UnityEngine.Vector2) relative to the bottom right corner of
the map.
