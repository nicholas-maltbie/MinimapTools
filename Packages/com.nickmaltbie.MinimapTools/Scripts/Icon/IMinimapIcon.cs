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

using nickmaltbie.MinimapTools.Minimap;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Icon
{
    /// <summary>
    /// Minimap Icon to display something on the minimap.
    /// </summary>
    public interface IMinimapIcon
    {
        /// <summary>
        /// Get the location of this object in world space.
        /// </summary>
        /// <returns>Position of the object in world space.</returns>
        Vector3 GetWorldSpace();

        /// <summary>
        /// Get the rotation of the object in world space.
        /// </summary>
        /// <returns>The current rotation of the object in world space.</returns>
        Quaternion GetIconRotation();

        /// <summary>
        /// Should the element rotate with the map.
        /// </summary>
        /// <returns>True if the element should rotate with the map, false otherwise.</returns>
        bool RotateWithMap();

        /// <summary>
        /// Should the icon scale with the map scale.
        /// </summary>
        /// <returns>If true, will scale with the map, if false will not scale
        /// with the map.</returns>
        bool ScaleWithMap();

        /// <summary>
        /// If this element scales with the map size, this will scale
        /// the element to be larger or smaller based on its world size.
        /// </summary>
        /// <returns>Size of the object in world units.</returns>
        Vector2 GetWorldSize();

        /// <summary>
        /// Create an icon to represent this object on the minimap.
        /// </summary>
        /// <param name="minimap">Reference to minimap this icon will be added to,
        /// useful for scaling and manipulating the icon</param>
        /// <param name="minimapTransform">Transform of the parent minimap this is being added to.</param>
        /// <returns>The newly created icon for this object.</returns>
        GameObject CreateIcon(IMinimap minimap, RectTransform minimapTransform);
    }
}
