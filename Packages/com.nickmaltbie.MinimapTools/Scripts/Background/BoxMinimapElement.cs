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

using com.nickmaltbie.MinimapTools.Minimap;
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Background
{
    /// <summary>
    /// Minimap element that will draw a box based on a
    /// set of bounds from a given box collider.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class BoxMinimapElement : AbstractMinimapElement
    {
        /// <summary>
        /// Color of the box on the minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Color of the box on the minimap.")]
        public Color color = Color.black;

        /// <inheritdoc/>
        public override Texture2D GetTexture(IMinimap minimap)
        {
            // Get the size of the box relative to the size of the minimap
            Bounds minimapBounds = minimap.GetWorldBounds();
            Vector3 boxSize = GetComponent<BoxCollider>().size;
            
            float relativeWidth = boxSize.x * transform.lossyScale.x / minimapBounds.size.x;
            float relativeHeight = boxSize.z * transform.lossyScale.z / minimapBounds.size.z;

            Vector2Int mapSize = minimap.GetSize();

            Texture2D texture = new Texture2D(
                Mathf.RoundToInt(relativeWidth * mapSize.x),
                Mathf.RoundToInt(relativeHeight * mapSize.y));

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            return texture;
        }
    }
}