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

using com.nickmaltbie.MinimapTools.Background;
using com.nickmaltbie.MinimapTools.Minimap.Shape;
using UnityEditor;
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.EditorTools
{
    /// <summary>
    /// Editor tool to modify bounds of a minimap.
    /// </summary>
    [CustomEditor(typeof(BoxMinimapElement))]
    public class BoxMinimapElementEditor : Editor
    {
        public void OnSceneGUI()
        {
            var minimapBox = (target as BoxMinimapElement);
            BoxCollider boxCollider = minimapBox.GetComponent<BoxCollider>();

            float rotation = minimapBox.transform.rotation.eulerAngles.y;

            MinimapSquare minimapShape = new MinimapSquare(
                new Vector2(
                    minimapBox.WorldCenter().x,
                    minimapBox.WorldCenter().z),
                new Vector2(
                    boxCollider.size.x * minimapBox.transform.lossyScale.x,
                    boxCollider.size.z * minimapBox.transform.lossyScale.z),
                -rotation);

            minimapBox.transform.rotation = Quaternion.Euler(0, rotation, 0);

            MinimapSquareUtils.RenderMinimapSquare(minimapShape, minimapBox.transform.position.y);
        }
    }
}
