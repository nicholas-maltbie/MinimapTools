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

using System.Linq;
using nickmaltbie.MinimapTools.Utils;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Utils
{
    /// <summary>
    /// Tests for the <see cref="nickmaltbie.MinimapTools.Utils.TextureUtils"/>
    /// </summary>
    [TestFixture]
    public class TextureUtilsTests : TestBase
    {
        private static readonly Color[] TestColors = new Color[] { Color.cyan, Color.yellow };

        [Test]
        public void Validate_CreateTexture(
            [ValueSource(nameof(TestColors))] Color color,
            [NUnit.Framework.Range(5, 20, 5)] int width,
            [NUnit.Framework.Range(5, 20, 5)] int height)
        {
            Texture2D texture = TextureUtils.CreateTexture(width, height, color);
            Assert.AreEqual(width, texture.width);
            Assert.AreEqual(height, texture.height);
            Assert.IsTrue(texture.GetPixels().All(c =>
            {
                TestUtils.AssertInBounds(c, color);
                return true;
            }));
        }

        [Test]
        public void Validate_StampTexture(
            [Values(0, 10, 50)] int offsetX,
            [Values(0, 10, 50)] int offsetY)
        {
            Texture2D source = TextureUtils.CreateTexture(100, 100);
            Texture2D stamp = TextureUtils.CreateTexture(10, 10, Color.red);

            source.DrawStamp(stamp, new Vector2Int(offsetX, offsetY));

            // Assert that pixels between (0,0) to (10,10) are all red now
            for (int x = 0; x < source.width; x++)
            {
                for (int y = 0; y < source.height; y++)
                {
                    if (x >= offsetX - 5 && x < offsetX + 5 &&
                        y >= offsetY - 5 && y < offsetY + 5)
                    {
                        Assert.AreEqual(source.GetPixel(x, y), Color.red);
                    }
                    else
                    {
                        Assert.AreEqual(source.GetPixel(x, y), Color.clear);
                    }
                }
            }
        }

        [Test]
        public void Validate_Texture_Rotate_Random(
            [NUnit.Framework.Range(0, 180, 57)] float rotation,
            [NUnit.Framework.Range(10, 40, 13)] int width,
            [NUnit.Framework.Range(10, 40, 13)] int height
        )
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels(
                Enumerable.Range(0, width * height)
                    .Select(_ => Random.ColorHSV())
                    .ToArray());

            Texture2D rotated = texture.GetRotated(rotation);

            Assert.GreaterOrEqual(rotated.width, Mathf.Min(width, height));
            Assert.GreaterOrEqual(rotated.width, Mathf.Min(width, height));
        }

        [Test]
        public void Validate_Texture_Rotate_Range(
            [NUnit.Framework.Range(-90, 90, 15)] float rotation
        )
        {
            var texture = new Texture2D(10, 10);
            texture.SetPixels(
                Enumerable.Range(0, 100)
                    .Select(_ => Random.ColorHSV())
                    .ToArray());

            Texture2D rotated = texture.GetRotated(rotation);

            int newSize = Mathf.CeilToInt(10.0f * (Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * rotation)) + Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * rotation))));
            Assert.AreEqual(newSize, rotated.width, 1);
            Assert.AreEqual(newSize, rotated.width, 1);
        }

        [Test]
        public void Validate_StampTexture_Relative(
            [Values(0, 10, 50)] int offsetX,
            [Values(0, 10, 50)] int offsetY)
        {
            Texture2D source = TextureUtils.CreateTexture(100, 100);
            Texture2D stamp = TextureUtils.CreateTexture(10, 10, Color.red);

            source.DrawStampRelative(stamp, new Vector2(offsetX / 100.0f, offsetY / 100.0f));

            // Assert that pixels between (0,0) to (10,10) are all red now
            for (int x = 0; x < source.width; x++)
            {
                for (int y = 0; y < source.height; y++)
                {
                    if (x >= offsetX - 5 && x < offsetX + 5 &&
                        y >= offsetY - 5 && y < offsetY + 5)
                    {
                        Assert.AreEqual(source.GetPixel(x, y), Color.red);
                    }
                    else
                    {
                        Assert.AreEqual(source.GetPixel(x, y), Color.clear);
                    }
                }
            }
        }

        [Test]
        public void Validate_StampTexture_Clear(
            [Values(10, 50)] int offsetX,
            [Values(10, 50)] int offsetY)
        {
            Texture2D source = TextureUtils.CreateTexture(100, 100, Color.red);
            Texture2D stamp = TextureUtils.CreateTexture(10, 10, Color.clear);

            source.DrawStampRelative(stamp, new Vector2(offsetX / 100.0f, offsetY / 100.0f));

            // Assert that pixels between (0,0) to (10,10) are all red now
            for (int x = 0; x < source.width; x++)
            {
                for (int y = 0; y < source.height; y++)
                {
                    Assert.AreEqual(source.GetPixel(x, y), Color.red);
                }
            }
        }
    }
}
