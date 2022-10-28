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

using nickmaltbie.MinimapTools.Background;
using UnityEditor;
using UnityEngine;

namespace nickmaltbie.MinimapTools.EditorTools
{
    /// <summary>
    /// Editor tool to modify size of a SpriteMinimapElement.
    /// </summary>
    [CustomEditor(typeof(SpriteMinimapElement))]
    public class SpriteMinimapElementEditor : Editor
    {
        public void OnSceneGUI()
        {
            var spriteElement = (target as SpriteMinimapElement);
            float rotation = spriteElement.transform.rotation.eulerAngles.y;

            spriteElement.elementBounds.center = new Vector2(spriteElement.transform.position.x, spriteElement.transform.position.z);
            spriteElement.elementBounds.rotation = -rotation % 360;

            spriteElement.transform.rotation = Quaternion.Euler(0, rotation, 0);

            MinimapSquareUtils.RenderMinimapSquare(spriteElement.elementBounds, spriteElement.transform.position.y);

            MinimapSquareUtils.DrawSizeHandle(spriteElement.elementBounds, spriteElement.transform, Vector2.right, spriteElement);
            MinimapSquareUtils.DrawSizeHandle(spriteElement.elementBounds, spriteElement.transform, Vector2.left, spriteElement);
            MinimapSquareUtils.DrawSizeHandle(spriteElement.elementBounds, spriteElement.transform, Vector2.up, spriteElement);
            MinimapSquareUtils.DrawSizeHandle(spriteElement.elementBounds, spriteElement.transform, Vector2.down, spriteElement);
        }
    }
}
