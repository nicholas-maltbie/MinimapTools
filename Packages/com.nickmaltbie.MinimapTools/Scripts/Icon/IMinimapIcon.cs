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

using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Icon
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
        /// Create an icon to represent this object on the minimap.
        /// </summary>
        /// <param name="minimapTransform">Transform of the parent minimap this is being added to.</param>
        /// <returns>The newly created icon for this object.</returns>
        GameObject CreateIcon(RectTransform minimapTransform);
    }
}
