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
    /// MinimapSquareUtils for the minimap.
    /// </summary>
    public static class MinimapSquareUtils
    {
        /// <summary>
        /// Render a minimap square in the editor space.
        /// </summary>
        /// <param name="minimapSquare">Minimap square to render.</param>
        /// <param name="offset">Offset of minimap square.</param>
        /// <param name="color">Color to draw the square for the minimap.</param>
        public static void RenderMinimapSquare(MinimapSquare minimapSquare, Vector3 offset, Color? color = null)
        {
            Vector3[] corners = minimapSquare.GetWorldSpaceCorners(offset);

            Handles.color = color ?? Color.red;

            Handles.DrawLine(corners[0], corners[1]);
            Handles.DrawLine(corners[1], corners[2]);
            Handles.DrawLine(corners[2], corners[3]);
            Handles.DrawLine(corners[3], corners[0]);
        }

        public static void UpdateMinimapSquareFromTransform(Transform transform, MinimapSquare minimapShape)
        {
            minimapShape.rotation = transform.rotation;

            // Update center in minimap position
            minimapShape.center = transform.position;
        }

        public static void DrawFourSizeHandles(MinimapSquare minimapSquare, Transform origin, Object objectToUndo = null)
        {
            MinimapSquareUtils.DrawSizeHandle(minimapSquare, origin, Vector2.right, objectToUndo);
            MinimapSquareUtils.DrawSizeHandle(minimapSquare, origin, Vector2.left, objectToUndo);
            MinimapSquareUtils.DrawSizeHandle(minimapSquare, origin, Vector2.up, objectToUndo);
            MinimapSquareUtils.DrawSizeHandle(minimapSquare, origin, Vector2.down, objectToUndo);
        }

        public static void DrawSizeHandle(MinimapSquare minimapSquare, Transform origin, Vector2 direction, Object objectToUndo = null)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 dir = origin.rotation * new Vector3(direction.x, 0, direction.y);
            Vector3 handlePos = origin.position + dir * Mathf.Abs(Vector2.Dot(minimapSquare.size, direction)) / 2;
            float size = HandleUtility.GetHandleSize(handlePos) * 0.5f;

            Vector3 newTargetPosition = Handles.Slider(handlePos, dir, size, Handles.ConeHandleCap, 0.1f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(objectToUndo ?? origin.gameObject, "Adjust size of minimap bounds");
                Vector3 move = newTargetPosition - handlePos;
                Vector2 delta = minimapSquare.ConvertToMinimapPlane(move);
                delta.Scale(direction);

                bool lockRatio = minimapSquare.lockAspectRatio;

                float ratio = minimapSquare.size.x / minimapSquare.size.y;

                if (lockRatio)
                {
                    if (delta.x == 0)
                    {
                        delta.x = delta.y * ratio;
                    }
                    else if (delta.y == 0)
                    {
                        delta.y = delta.x / ratio;
                    }
                }

                minimapSquare.size += delta;
                minimapSquare.size.x = Mathf.Max(1, minimapSquare.size.x);
                minimapSquare.size.y = Mathf.Max(1, minimapSquare.size.y);
            }
        }
    }
}
