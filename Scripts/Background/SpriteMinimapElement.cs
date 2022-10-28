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
using nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.MinimapTools.Utils;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Background
{
    /// <summary>
    /// Minimap element that will draw a box based on a
    /// set of bounds from a given box collider.
    /// </summary>
    public class SpriteMinimapElement : AbstractMinimapElement
    {
        /// <summary>
        /// Texture to draw on the minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Texture to draw on the minimap.")]
        internal Texture2D elementTexture;

        /// <summary>
        /// Bounds of the sprite on the minimap.
        /// </summary>
        [SerializeField]
        internal MinimapSquare elementBounds;

        /// <summary>
        /// Color of the box on the minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Color of the box on the minimap.")]
        public Color color = Color.black;

        /// <inheritdoc/>
        public override Texture2D GetTexture(IMinimap minimap)
        {
            Texture2D texture = elementTexture.GetResized(new Vector2Int(
                Mathf.RoundToInt(elementBounds.Size.x * minimap.PixelsPerUnit),
                Mathf.RoundToInt(elementBounds.Size.y * minimap.PixelsPerUnit)));

            return texture;
        }

        /// <inheritdoc/>
        public override Vector3 WorldCenter()
        {
            return transform.position;
        }

        /// <inheritdoc/>
        public override float GetRotation()
        {
            return -elementBounds.rotation + 180;
        }
    }
}
