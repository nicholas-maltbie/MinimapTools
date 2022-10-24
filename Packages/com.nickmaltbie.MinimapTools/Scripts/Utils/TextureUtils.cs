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

namespace com.nickmaltbie.MinimapTools.Background
{
    /// <summary>
    /// Utility functions for texture utils.
    /// </summary>
    public static class TextureUtils
    {
        /// <summary>
        /// Get a resized Texture2D via GPU and RenderTexture.
        /// </summary>
        /// <param name="texture">Texture to resize.</param>
        /// <param name="size">Size to scale texture to.</param>
        /// <param name="depth">Number of values per pixel, 24 for rgb, 32 for rgba</param>
        /// <returns>Newly created resized texture.</returns>
        public static Texture2D GetResized(this Texture2D texture, Vector2Int size, int depth=32)
        {
            RenderTexture renderTexture = new RenderTexture(size.x, size.y, 32);
            RenderTexture.active = renderTexture;
            Graphics.Blit(texture, renderTexture);
            Texture2D result=new Texture2D(size.x, size.y);
            result.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
            result.Apply();
            return result;
        }

        /// <summary>
        /// Draw a stamp at a relative position onto a texture map.
        /// </summary>
        /// <param name="source">Source texture to draw stamp on.</param>
        /// <param name="stamp">Stamp texture to draw on source.</param>
        /// <param name="relativePosition">Relative position of the 
        /// center of the stamp on the source texture.</param>
        public static void DrawStampRelative(this Texture2D source, Texture2D stamp, Vector2 relativePosition)
        {
            DrawStamp(source, stamp, new Vector2Int(
                (int) Mathf.Round(relativePosition.x * source.width),
                (int) Mathf.Round(relativePosition.y * source.height)));
        }

        /// <summary>
        /// Draw a stamp texture on a base texture map given a specific offset.
        /// </summary>
        /// <param name="source">Source texture to draw stamp on.</param>
        /// <param name="stamp">Stamp texture to draw on source.</param>
        /// <param name="offsetPixels">Center offset of stamp on the source image.</param>
        public static void DrawStamp(this Texture2D source, Texture2D stamp, Vector2Int offsetPixels)
        {
            Debug.Log($"Drawing stamp (size:{stamp.width},{stamp.height}) on source (size:{source.width},{source.height}) at offset {offsetPixels.x},{offsetPixels.y}");

            // Go through each pixel in the stamp
            for (int x = 0; x < stamp.width; x++)
            {
                for (int y = 0; y < stamp.height; y++)
                {
                    // Identify where this pixel would be located on the target image
                    int targetX = x + offsetPixels.x - stamp.width / 2;
                    int targetY = y + offsetPixels.y - stamp.height / 2;

                    // If the target is not in the base image, skip this pixel
                    if (targetX < 0 || targetX >= source.width ||
                        targetY < 0 || targetY >= source.height)
                    {
                        continue;
                    }

                    // Combine the source and target pixel based on the alpha
                    // value of the stamp pixel.
                    Color stampPixel = stamp.GetPixel(x, y);
                    Color bgPixel = source.GetPixel(targetX, targetY);
                    source.SetPixel(targetX, targetY, Color.Lerp(bgPixel, stampPixel, stampPixel.a));
                }
            }

            source.Apply();
        }
    }
}
