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

using nickmaltbie.MinimapTools.Minimap.Shape;
using UnityEditor;
using UnityEngine;

namespace nickmaltbie.MinimapTools.EditorTools
{
    /// <summary>
    /// Editor tool to modify bounds of a minimap.
    /// </summary>
    [CustomEditor(typeof(MinimapBoundsSource))]
    public class MinimapBoundsSourceEditor : Editor
    {
        public void OnSceneGUI()
        {
            var minimapBounds = (target as MinimapBoundsSource);
            float rotation = minimapBounds.transform.rotation.eulerAngles.y;

            minimapBounds.minimapShape.center = new Vector2(minimapBounds.transform.position.x, minimapBounds.transform.position.z);
            minimapBounds.minimapShape.rotation = -rotation % 360;

            minimapBounds.transform.rotation = Quaternion.Euler(0, rotation, 0);

            MinimapSquareUtils.RenderMinimapSquare(minimapBounds.minimapShape, minimapBounds.transform.position.y);

            MinimapSquareUtils.DrawSizeHandle(minimapBounds.minimapShape, minimapBounds.transform, Vector2.right, minimapBounds);
            MinimapSquareUtils.DrawSizeHandle(minimapBounds.minimapShape, minimapBounds.transform, Vector2.left, minimapBounds);
            MinimapSquareUtils.DrawSizeHandle(minimapBounds.minimapShape, minimapBounds.transform, Vector2.up, minimapBounds);
            MinimapSquareUtils.DrawSizeHandle(minimapBounds.minimapShape, minimapBounds.transform, Vector2.down, minimapBounds);
        }
    }
}
