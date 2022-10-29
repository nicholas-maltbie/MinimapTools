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
using System.Linq;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Utils
{
    /// <summary>
    /// Utility functions for texture utils.
    /// </summary>
    public static class TextureUtils
    {
        /// <summary>
        /// Rotates a texture about its center.
        /// </summary>
        /// <param name="texture">Texture to rotate.</param>
        /// <param name="rotation">Rotation of the image in degrees.</param>
        /// <returns>Rotated texture.</returns>
        public static Texture2D GetRotated(this Texture2D texture, float rotation)
        {
            float cos = Mathf.Cos(rotation * Mathf.Deg2Rad);
            float sin = Mathf.Sin(rotation * Mathf.Deg2Rad);

            float c1x, c1y, c2x, c2y, c3x, c3y, c4x, c4y;
            (c1x, c1y) = MathUtils.GetRotatedAboutOrigin(texture.width / 2.0f, texture.height / 2.0f, rotation);
            (c2x, c2y) = MathUtils.GetRotatedAboutOrigin(texture.width / 2.0f, -texture.height / 2.0f, rotation);
            (c3x, c3y) = MathUtils.GetRotatedAboutOrigin(-texture.width / 2.0f, texture.height / 2.0f, rotation);
            (c4x, c4y) = MathUtils.GetRotatedAboutOrigin(-texture.width / 2.0f, -texture.height / 2.0f, rotation);

            int minX = Mathf.FloorToInt(Mathf.Min(c1x, c2x, c3x, c4x));
            int maxX = Mathf.CeilToInt(Mathf.Max(c1x, c2x, c3x, c4x));
            int minY = Mathf.FloorToInt(Mathf.Min(c1y, c2y, c3y, c4y));
            int maxY = Mathf.CeilToInt(Mathf.Max(c1y, c2y, c3y, c4y));
            int width = maxX - minX;
            int height = maxY - minY;

            var rotated = new Texture2D(width, height);
            rotated.Apply();

            Color?[] colorTable = Enumerable.Repeat<Color?>(null, width * height).ToArray();

            Action<int, int, float, Color> UpdateColor = (int x, int y, float weight, Color source) =>
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    return;
                }

                int idx = x + y * width;
                Color? current = colorTable[idx];
                if (current == null)
                {
                    colorTable[idx] = new Color(source.r, source.g, source.b, weight * source.a);
                }
                else
                {
                    var blended = Color.Lerp(current.Value, source, source.a * weight);
                    blended.a += weight * source.a;
                    colorTable[idx] = blended;
                }
            };

            for (int sourceX = 0; sourceX < texture.width; sourceX++)
            {
                for (int sourceY = 0; sourceY < texture.height; sourceY++)
                {
                    Color sourceColor = texture.GetPixel(sourceX, sourceY);
                    float x = (sourceX - texture.width / 2) * cos - (sourceY - texture.height / 2) * sin + width / 2;
                    float y = (sourceX - texture.width / 2) * sin + (sourceY - texture.height / 2) * cos + height / 2;

                    int x1 = Mathf.FloorToInt(x);
                    int y1 = Mathf.FloorToInt(y);
                    int x2 = Mathf.CeilToInt(x);
                    int y2 = Mathf.CeilToInt(y);

                    bool onX = x - x1 <= 0.000001f && x2 - x <= 0.000001f;
                    bool onY = y - y1 <= 0.000001f && y2 - y <= 0.000001f;

                    if (onX && onY)
                    {
                        int x_ = Mathf.RoundToInt(x);
                        int y_ = Mathf.RoundToInt(y);
                        UpdateColor(x1, y1, 1, sourceColor);
                        continue;
                    }
                    else if (onX && !onY)
                    {
                        int x_ = Mathf.RoundToInt(x);
                        float w1 = (y2 - y) / (y2 - y1);
                        float w2 = (y - y1) / (y2 - y1);
                        UpdateColor(x_, y1, w1, sourceColor);
                        UpdateColor(x_, y2, w2, sourceColor);
                        continue;
                    }
                    else if (!onX && onY)
                    {
                        int y_ = Mathf.RoundToInt(y);
                        float w1 = (x2 - x) / (x2 - x1);
                        float w2 = (x - x1) / (x2 - x1);
                        UpdateColor(x1, y_, w1, sourceColor);
                        UpdateColor(x2, y_, w2, sourceColor);
                        continue;
                    }

                    // Set the four pixels closest to the dest with some anti-aliasing
                    float w11 = (x2 - x) * (y2 - y) / (x2 - x1) / (y2 - y1);
                    float w12 = (x2 - x) * (y - y1) / (x2 - x1) / (y2 - y1);
                    float w21 = (x - x1) * (y2 - y) / (x2 - x1) / (y2 - y1);
                    float w22 = (x - x1) * (y - y1) / (x2 - x1) / (y2 - y1);

                    UpdateColor(x1, y1, w11, sourceColor);
                    UpdateColor(x1, y2, w12, sourceColor);
                    UpdateColor(x2, y1, w21, sourceColor);
                    UpdateColor(x2, y2, w22, sourceColor);
                }
            }

            rotated.SetPixels(colorTable.Select(color => color.HasValue ? color.Value : Color.clear).ToArray());
            rotated.Apply();
            return rotated;
        }

        /// <summary>
        /// Get a resized Texture2D via GPU and RenderTexture.
        /// </summary>
        /// <param name="texture">Texture to resize.</param>
        /// <param name="size">Size to scale texture to.</param>
        /// <param name="depth">Number of values per pixel, 24 for rgb, 32 for rgba</param>
        /// <returns>Newly created resized texture.</returns>
        public static Texture2D GetResized(this Texture2D texture, Vector2Int size, int depth = 32)
        {
            var renderTexture = new RenderTexture(size.x, size.y, depth);
            RenderTexture.active = renderTexture;
            Graphics.Blit(texture, renderTexture);
            var result = new Texture2D(size.x, size.y);
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
            source.DrawStamp(
                stamp,
                new Vector2Int(
                    Mathf.RoundToInt(relativePosition.x * source.width),
                    Mathf.RoundToInt(relativePosition.y * source.height)));
        }

        /// <summary>
        /// Create a texture of a given size with a given color.
        /// </summary>
        /// <param name="width">Width fo texture.</param>
        /// <param name="height">Height of texture</param>
        /// <param name="color">Color to make texture.</param>
        public static Texture2D CreateTexture(int width, int height, Color? color = null)
        {
            Color bg = color ?? Color.clear;
            var texture = new Texture2D(width, height);
            texture.SetPixels(Enumerable.Repeat(bg, width * height).ToArray());
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Draw a stamp texture on a base texture map given a specific offset.
        /// </summary>
        /// <param name="source">Source texture to draw stamp on.</param>
        /// <param name="stamp">Stamp texture to draw on source.</param>
        /// <param name="offsetPixels">Center offset of stamp on the source image.</param>
        public static void DrawStamp(this Texture2D source, Texture2D stamp, Vector2Int offsetPixels)
        {
            // Go through each pixel in the stamp
            for (int x = 0; x < stamp.width; x++)
            {
                for (int y = 0; y < stamp.height; y++)
                {
                    // Identify where this pixel would be located on the target image
                    int targetX = x - stamp.width / 2 + offsetPixels.x;
                    int targetY = y - stamp.height / 2 + offsetPixels.y;

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
