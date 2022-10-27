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
using nickmaltbie.MinimapTools.Utils;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Background
{
    /// <summary>
    /// Element to compose a background texture from a collection of
    /// minimap elements.
    /// </summary>
    public class BackgroundTexture
    {
        /// <summary>
        /// Generated texture for the background.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Reference to minimap for the texture.
        /// </summary>
        private IMinimap minimap;

        /// <summary>
        /// Create a background texture.
        /// </summary>
        /// <param name="minimap">Minimap reference.</param>
        /// <param name="size">Size of texture in pixels.</param>
        /// <param name="background">Background of the minimap.</param>
        public BackgroundTexture(IMinimap minimap, Vector2Int size, Texture2D background)
        {
            if (background != null)
            {
                texture = background.GetResized(size);
            }
            else
            {
                texture = TextureUtils.CreateTexture(size.x, size.y);
            }

            this.minimap = minimap;
        }

        /// <summary>
        /// Create a background texture.
        /// </summary>
        /// <param name="minimap">Minimap reference.</param>
        /// <param name="size">Size of texture in pixels.</param>
        /// <param name="background">Background color of the minimap.</param>
        public BackgroundTexture(IMinimap minimap, Vector2Int size, Color? color = null)
        {
            texture = TextureUtils.CreateTexture(size.x, size.y, color);
            this.minimap = minimap;
        }

        /// <summary>
        /// Add a given element to the minimap.
        /// </summary>
        /// <param name="element">Element to add to the minimap.</param>
        public void AddElementToMinimap(IMinimapElement element)
        {
            element.DrawOnBackground(minimap, texture);
        }

        /// <summary>
        /// Get the Texture2D of this minimap.
        /// </summary>
        /// <returns>Texture of the minimap.</returns>
        public Texture2D GetTexture2D() => texture;
    }
}
