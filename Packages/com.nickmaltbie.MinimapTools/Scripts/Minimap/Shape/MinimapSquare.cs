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
using com.nickmaltbie.MinimapTools.Utils;
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Minimap.Shape
{
    /// <summary>
    /// Source of bounds for a minimap or minimap object.
    /// </summary>
    [Serializable]
    public class MinimapSquare : IMinimapShape
    {
        /// <summary>
        /// Center of the object in world space.
        /// </summary>
        [ReadOnly]
        public Vector2 center;

        /// <summary>
        /// Rotation of the object about the vertical axis.
        /// </summary>
        [ReadOnly]
        public float rotation;

        /// <summary>
        /// Size of the object in world space.
        /// </summary>
        public Vector2 size = Vector2.one;

        /// <summary>
        /// Should the aspect ratio of the size be locked.
        /// </summary>
        public bool lockAspectRatio;

        /// <summary>
        /// Create a minimap square with a given configuration.
        /// </summary>
        /// <param name="center">Center of the object in world space.</param>
        /// <param name="size">Size of the object in world space.</param>
        /// <param name="rotation">Rotation of the object about the vertical axis.</param>
        public MinimapSquare(Vector2 center, Vector2 size, float rotation)
        {
            this.center = center;
            this.size = size;
            this.rotation = rotation;
        }

        /// <inheritdoc/>
        public Vector2 Center => center;

        /// <inheritdoc/>
        public Vector2 Size => size;

        /// <inheritdoc/>
        public Vector2 Min
        {
            get
            {
                float x1, y1, x2, y2, x3, y3, x4, y4;
                ((x1, y1), (x2, y2), (x3, y3), (x4, y4)) = GetCorners();
                return new Vector2(Mathf.Min(x1, x2, x3, x4), Mathf.Min(y1, y2, y3, y4));
            }
        }

        /// <inheritdoc/>
        public Vector2 Max
        {
            get
            {
                float x1, y1, x2, y2, x3, y3, x4, y4;
                ((x1, y1), (x2, y2), (x3, y3), (x4, y4)) = GetCorners();
                return new Vector2(Mathf.Max(x1, x2, x3, x4), Mathf.Max(y1, y2, y3, y4));
            }
        }

        /// <inheritdoc/>
        public bool Contains(Vector2 point)
        {
            float dx, dy;
            (dx, dy) = GetPositionRelativeToP1(point);
            return dx >= 0 && dx <= size.x && dy >= 0 && dy <= size.y;
        }

        /// <summary>
        /// https://math.stackexchange.com/questions/2157931/how-to-check-if-a-point-is-inside-a-square-2d-plane
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public (float, float) GetPositionRelativeToP1(Vector2 point)
        {
            float x1, x2, x4, y1, y2, y4;
            ((x1, y1), (x2, y2), _, (x4, y4)) = GetCorners();

            // Define vector for bottom and left edge
            float xbot, ybot, xleft, yleft;
            xbot = x4 - x1;
            ybot = y4 - y1;
            xleft = x2 - x1;
            yleft = y2 - y1;

            // Define vector from point point to bottom left corner
            float diagx, diagy;
            diagx = point.x - x1;
            diagy = point.y - y1;

            // Compute the horizontal and vertical components of distance
            // relative to the bottom left corner.
            float dx = MathUtils.Dot(diagx, diagy, xbot, ybot) / MathUtils.Length(xbot, ybot);
            float dy = MathUtils.Dot(diagx, diagy, xleft, yleft) / MathUtils.Length(xleft, yleft);

            return (dx, dy);
        }

        /// <summary>
        /// Get the four corners of this minimap square rotated about the center.
        /// <pre>
        ///     [1] x2,y2 --- [2] x3,y3
        ///      |                   |
        ///     [0] x1,y1 --- [3] x4,y4
        /// </pre>
        /// </summary>
        /// <returns>Corners of the minimap square rotated about the center.
        /// Returns the points in order ((x1, y1), (x2, y2), (x3, y3), (x4, y4))</returns>
        public ((float, float), (float, float), (float, float), (float, float)) GetCorners()
        {
            return (
                MathUtils.GetRotatedPoint(center.x - size.x / 2, center.y - size.y / 2, center.x, center.y, rotation),
                MathUtils.GetRotatedPoint(center.x - size.x / 2, center.y + size.y / 2, center.x, center.y, rotation),
                MathUtils.GetRotatedPoint(center.x + size.x / 2, center.y + size.y / 2, center.x, center.y, rotation),
                MathUtils.GetRotatedPoint(center.x + size.x / 2, center.y - size.y / 2, center.x, center.y, rotation)
            );
        }
    }
}
