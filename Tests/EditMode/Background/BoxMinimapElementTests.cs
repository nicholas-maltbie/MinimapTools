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

using System.Collections.Generic;
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
    /// Tests for the <see cref="nickmaltbie.MinimapTools.Background.BoxMinimapElement"/>
    /// </summary>
    [TestFixture]
    public class BoxMinimapElementTests : TestBase
    {
        private BoxMinimapElement boxMinimapElement;
        private BoxCollider boxCollider;
        private Texture2D backgroundTexture;
        private Mock<IMinimap> minimapMock;
        private MinimapSquare minimapSquare;

        [SetUp]
        public void SetUp()
        {
            GameObject go = base.CreateGameObject();
            go.transform.localScale = new Vector3(4, 1, 8);

            boxMinimapElement = go.AddComponent<BoxMinimapElement>();
            boxCollider = go.GetComponent<BoxCollider>();

            minimapMock = new Mock<IMinimap>();
            minimapSquare = new MinimapSquare(Vector2.zero, Vector2.one * 10, Quaternion.identity);

            backgroundTexture = new Texture2D(100, 100);
            backgroundTexture.SetPixels(
                Enumerable.Repeat(Color.clear, backgroundTexture.width * backgroundTexture.height).ToArray());
            backgroundTexture.Apply();

            minimapMock.Setup(m => m.GetWorldBounds()).Returns(minimapSquare);
            minimapMock.Setup(m => m.GetRotation()).Returns(0);
            minimapMock.Setup(m => m.PixelsPerUnit).Returns(10);
            minimapMock.Setup(m => m.GetSize()).Returns(new Vector2Int(backgroundTexture.width, backgroundTexture.height));
            minimapMock.Setup(m => m.GetMinimapPosition(It.IsAny<Vector3>()))
                .Returns((Vector3 v) => new Vector2(0.5f, 0.5f));

            boxCollider.size = Vector3.one;
        }

        public static IEnumerable<Color> TestColors = new[] { Color.red, Color.blue, Color.green };

        [Test]
        public void Validate_BoxMinimapElement_DrawOnBackground([ValueSource(nameof(TestColors))] Color color)
        {
            boxMinimapElement.color = color;
            boxMinimapElement.DrawOnBackground(minimapMock.Object, backgroundTexture);

            // Box should be drawn on pixel (0,0), assert that the color
            // is found there
            Assert.AreEqual(
                color,
                backgroundTexture.GetPixel(
                    backgroundTexture.width / 2,
                    backgroundTexture.height / 2));
        }

        [Test]
        public void Validate_BoxMinimapElement_DrawOnBackground_Rotated([ValueSource(nameof(TestColors))] Color color)
        {
            boxMinimapElement.color = color;
            boxMinimapElement.DrawOnBackground(minimapMock.Object, backgroundTexture);
            boxMinimapElement.transform.rotation = Quaternion.Euler(0, 30, 0);

            // Box should be drawn on pixel (0,0), assert that the color
            // is found there
            Assert.AreEqual(
                color,
                backgroundTexture.GetPixel(
                    backgroundTexture.width / 2,
                    backgroundTexture.height / 2));
        }

        [Test]
        public void Validate_BoxMinimapElement_GetTexture([ValueSource(nameof(TestColors))] Color color)
        {
            boxMinimapElement.color = color;
            Texture2D texture = boxMinimapElement.GetTexture(minimapMock.Object);
            Assert.AreEqual(boxCollider.size.x * boxCollider.transform.lossyScale.x, texture.width / minimapMock.Object.PixelsPerUnit);
            Assert.AreEqual(boxCollider.size.y * boxCollider.transform.lossyScale.z, texture.height / minimapMock.Object.PixelsPerUnit);
            Assert.IsTrue(texture.GetPixels().All(c => c.Equals(color)));
        }
    }
}
