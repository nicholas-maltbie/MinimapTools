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

using System;
using nickmaltbie.MinimapTools.Utils;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Minimap.Shape
{
    /// <summary>
    /// Source of bounds for a minimap or minimap object.
    /// </summary>
    [Serializable]
    public class MinimapSquare : IMinimapShape
    {
        /// <summary>
        /// Center of the object in minimap space.
        /// </summary>
        [ReadOnly]
        public Vector3 center;

        /// <summary>
        /// Rotation of the object about the vertical axis.
        /// </summary>
        [ReadOnly]
        public Quaternion rotation;

        /// <summary>
        /// Size of the object in world space.
        /// </summary>
        public Vector2 size;

        /// <summary>
        /// Should the aspect ratio of the size be locked.
        /// </summary>
        public bool lockAspectRatio;

        /// <summary>
        /// Create a minimap square with a given configuration.
        /// </summary>
        /// <param name="center">Center of the object in world space.</param>
        /// <param name="size">Size of the object in world space.</param>
        /// <param name="rotation">Rotation of the object in 3D space for the plane.</param>
        public MinimapSquare(Vector3 center, Vector3 size, Quaternion rotation)
        {
            this.center = center;
            this.size = size;
            this.rotation = rotation;
        }

        /// <inheritdoc/>
        public Vector3 Center => center;

        /// <inheritdoc/>
        public Vector2 Size => size;

        /// <summary>
        /// Get the position of an element from world space to minimap space.
        /// </summary>
        /// <param name="worldSpace">Position in world space.</param>
        /// <returns>Position in minimap space.</returns>
        public Vector2 ConvertToMinimapPlane(Vector3 worldSpace)
        {
            Vector3 xComponent = Vector3.Project(worldSpace, MapAxisHoriz());
            Vector3 yComponent = Vector3.Project(worldSpace, MapAxisVert());
            float xSign = Vector3.Dot(xComponent, MapAxisHoriz()) > 0 ? 1 : -1;
            float ySign = Vector3.Dot(yComponent, MapAxisVert()) > 0 ? 1 : -1;

            return new Vector2(xComponent.magnitude * xSign, yComponent.magnitude * ySign);
        }

        /// <summary>
        /// Get the corners of the minimap in world space.
        /// </summary>
        /// <param name="offset">Offset from center in world space.</param>
        /// <returns>Four corners of the minimap as vector 3 in world space.</returns>
        public Vector3[] GetWorldSpaceCorners(Vector3? offset)
        {
            offset ??= Vector3.zero;
            Vector3 horiz = MapAxisHoriz();
            Vector3 vert = MapAxisVert();

            return new Vector3[]
            {
                offset.Value - horiz * size.x / 2 - vert * size.y / 2,
                offset.Value - horiz * size.x / 2 + vert * size.y / 2,
                offset.Value + horiz * size.x / 2 + vert * size.y / 2,
                offset.Value + horiz * size.x / 2 - vert * size.y / 2
            };
        }

        /// <inheritdoc/>
        public Vector3 MapNormal()
        {
            return rotation * Vector3.up;
        }

        /// <inheritdoc/>
        public Vector3 MapAxisHoriz()
        {
            return rotation * Vector3.right;
        }

        /// <inheritdoc/>
        public Vector3 MapAxisVert()
        {
            return rotation * Vector3.forward;
        }
    }
}
