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

namespace nickmaltbie.MinimapTools.Utils
{
    /// <summary>
    /// Utility functions for math equations and features.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Get the coordinates of a point rotated about the origin (0,0).
        /// </summary>
        /// <param name="x1">X value of point to rotate.</param>
        /// <param name="y1">Y value of point to rotate.</param>
        /// <param name="rotation">Amount to rotate point about origin in degrees.</param>
        /// <returns>Rotated position of point (x1, y1) about the origin (0,0). </returns>
        public static (float, float) GetRotatedAboutOrigin(float x1, float y1, float rotation)
        {
            return GetRotatedPoint(x1, y1, 0, 0, rotation);
        }

        /// <summary>
        /// Get the coordinates of a point rotated about a pivlot.
        /// </summary>
        /// <param name="x1">X value of point to rotate.</param>
        /// <param name="y1">Y value of point to rotate.</param>
        /// <param name="pivotX">X value of pivot.</param>
        /// <param name="pivotY">Y value of pivot.</param>
        /// <param name="rotation">Amount to rotate point about pivot in degrees.</param>
        /// <returns>Rotated position of point (x1, y1) about the pivot.</returns>
        public static (float, float) GetRotatedPoint(float x1, float y1, float pivotX, float pivotY, float rotation)
        {
            float cos = Mathf.Cos(rotation * Mathf.Deg2Rad);
            float sin = Mathf.Sin(rotation * Mathf.Deg2Rad);

            float x2 = (x1 - pivotX) * cos - (y1 - pivotY) * sin + pivotX;
            float y2 = (x1 - pivotX) * sin + (y1 - pivotY) * cos + pivotY;

            return (x2, y2);
        }
    }
}
