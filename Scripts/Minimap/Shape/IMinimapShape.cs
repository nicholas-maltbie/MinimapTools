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

namespace nickmaltbie.MinimapTools.Minimap.Shape
{
    /// <summary>
    /// Source of bounds for a minimap or minimap object.
    /// </summary>
    public interface IMinimapShape
    {
        /// <summary>
        /// Get the center of this minimap shape in world space.
        /// </summary>
        /// <value>Position of the object in world space.</value>
        Vector3 Center { get; }

        /// <summary>
        /// Get the size of this minimap object in world space.
        /// </summary>
        /// <value>2D size of the object in world space (only xz axis).</value>
        Vector2 Size { get; }

        /// <summary>
        /// Get the normal vector for the plane of this minimap.
        /// </summary>
        /// <returns>Vector normal to the minimap plane.</returns>
        Vector3 MapNormal();

        /// <summary>
        /// Direction of the axis horizontal along the minimap plane.
        /// </summary>
        /// <returns>Horizontal directional vector.</returns>
        Vector3 MapAxisHoriz();

        /// <summary>
        /// Direction of the vertical axis along the minimap plane.
        /// </summary>
        /// <returns>Vertical direction vector.</returns>
        Vector3 MapAxisVert();
    }
}
