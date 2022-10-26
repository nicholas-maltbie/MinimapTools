// Copyright (C) 2022 Nicholas Maltbie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using com.nickmaltbie.MinimapTools.Icon;
using com.nickmaltbie.MinimapTools.Minimap.Shape;
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Minimap
{
    /// <summary>
    /// Minimap for managing state.
    /// </summary>
    public interface IMinimap
    {
        /// <summary>
        /// Number of pixels per unit of the minimap size.
        /// </summary>
        /// <returns>Returns the number of pixels per unit for sizing the minimap.</returns>
        float PixelsPerUnit { get; }

        /// <summary>
        /// Check if a location is in the minimap space.
        /// </summary>
        /// <param name="worldSpace">World space of the object.</param>
        /// <returns>True if the location is within the minimap, false otherwise.</returns>
        bool InMap(Vector3 worldSpace);

        /// <summary>
        /// Get the rotation of the minimap with respect to the vertical axis.
        /// </summary>
        /// <returns>Rotation of the minimap in degrees.</returns>
        float GetRotation();

        /// <summary>
        /// Add an icon to the minimap.
        /// </summary>
        /// <param name="minimapIcon">Icon to add to the minimap.</param>
        /// <returns>True if the object was added successfully, false otherwise.</returns>
        bool AddIcon(IMinimapIcon minimapIcon);

        /// <summary>
        /// Remove an icon from the minimap.
        /// </summary>
        /// <param name="minimapIcon">Icon to remove to the minimap.</param>
        /// <returns>True if the object was removed successfully, false otherwise.</returns>
        bool RemoveIcon(IMinimapIcon minimapIcon);

        /// <summary>
        /// Translates a position from world space to normalized minimap space.
        /// </summary>
        /// <param name="worldPosition">Position of the object in world space.</param>
        /// <returns>Normalized minimap position, will scale positions within
        /// the minimap to between (0,0) and (1,1).</returns>
        Vector2 GetMinimapPosition(Vector3 worldPosition);

        /// <summary>
        /// Get the world bounds for the minimap.
        /// </summary>
        /// <returns>World bound that the minimap represents.</returns>
        IMinimapShape GetWorldBounds();

        /// <summary>
        /// Get the size of the minimap texture in pixels.
        /// </summary>
        /// <returns>Vector2Int of the width and height of the minimap.</returns>
        Vector2Int GetSize();
    }
}
