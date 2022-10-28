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
using Moq;
using nickmaltbie.MinimapTools.Background;
using nickmaltbie.MinimapTools.Minimap;
using nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Background
{
    /// <summary>
    /// Tests for the <see cref="nickmaltbie.MinimapTools.Background.SpriteMinimapElement"/>
    /// </summary>
    [TestFixture]
    public class SpriteMinimapElementTests : TestBase
    {
        private SpriteMinimapElement spriteMinimapElement;
        private Texture2D backgroundTexture;
        private Mock<IMinimap> minimapMock;
        private MinimapSquare minimapSquare;

        [SetUp]
        public void SetUp()
        {
            GameObject go = base.CreateGameObject();
            spriteMinimapElement = go.AddComponent<SpriteMinimapElement>();
            spriteMinimapElement.elementTexture = new Texture2D(10, 10);
            spriteMinimapElement.elementBounds = new MinimapSquare(Vector2.zero, Vector2.one, 0);

            // Fill sprite with random pixels
            spriteMinimapElement.elementTexture.SetPixels(
                Enumerable.Range(0, spriteMinimapElement.elementTexture.width * spriteMinimapElement.elementTexture.height)
                    .Select(_ => Random.ColorHSV())
                    .ToArray());
            spriteMinimapElement.elementTexture.Apply();

            minimapMock = new Mock<IMinimap>();
            minimapSquare = new MinimapSquare(Vector2.zero, Vector2.one * 10, 0);

            backgroundTexture = new Texture2D(100, 100);
            backgroundTexture.SetPixels(
                Enumerable.Repeat(Color.clear, backgroundTexture.width * backgroundTexture.height).ToArray());
            backgroundTexture.Apply();

            minimapMock.Setup(m => m.GetWorldBounds()).Returns(minimapSquare);
            minimapMock.Setup(m => m.GetRotation()).Returns(0);
            minimapMock.Setup(m => m.PixelsPerUnit).Returns(10);
            minimapMock.Setup(m => m.GetSize()).Returns(Vector2Int.one * 100);
            minimapMock.Setup(m => m.GetMinimapPosition(It.IsAny<Vector3>()))
                .Returns((Vector3 v) => Vector2.one * 0.5f);
        }

        [Test]
        public void Validate_SpriteMinimapElement_DrawOnBackground()
        {
            spriteMinimapElement.DrawOnBackground(minimapMock.Object, backgroundTexture);

            // Box should be drawn on pixel (0,0), assert that the color
            // is found there
            Assert.AreEqual(
                spriteMinimapElement.elementTexture.GetPixel(
                    spriteMinimapElement.elementTexture.width / 2,
                    spriteMinimapElement.elementTexture.height / 2
                ),
                backgroundTexture.GetPixel(
                    backgroundTexture.width / 2,
                    backgroundTexture.height / 2));
        }

        [Test]
        public void Validate_SpriteMinimapElement_GetTexture()
        {
            Texture2D texture = spriteMinimapElement.GetTexture(minimapMock.Object);
            Assert.AreEqual(spriteMinimapElement.elementBounds.size.x * 10, texture.width);
            Assert.AreEqual(spriteMinimapElement.elementBounds.size.y * 10, texture.height);
            Assert.IsTrue(Enumerable.SequenceEqual(
                texture.GetPixels(),
                spriteMinimapElement.elementTexture.GetPixels()));
        }
    }
}
