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

using Moq;
using nickmaltbie.MinimapTools.Icon;
using nickmaltbie.MinimapTools.Minimap;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Icon
{
    /// <summary>
    /// Tests for the <see cref="nickmaltbie.MinimapTools.Icon.RelativeSizeSpriteIcon"/>
    /// </summary>
    [TestFixture]
    public class RelativeSizeSpriteIconTests : TestBase
    {
        [Test]
        public void Validate_RelativeSizeSpriteIcon(
            [NUnit.Framework.Range(1, 5, 2)] int width,
            [NUnit.Framework.Range(1, 5, 2)] int height,
            [NUnit.Framework.Range(10, 20, 5)] int pixelScale
        )
        {
            RelativeSizeSpriteIcon icon = CreateGameObject().AddComponent<RelativeSizeSpriteIcon>();
            icon.worldSize = new Vector2Int(width, height);

            var minimapMock = new Mock<IMinimap>();
            minimapMock.Setup(m => m.PixelsPerUnit).Returns(pixelScale);

            TestUtils.AssertInBounds(icon.worldSize * pixelScale, icon.GetPixelSize(minimapMock.Object));
            Assert.AreEqual(icon.ScaleWithMap(), true);
            Assert.AreEqual(icon.GetWorldSize(), icon.worldSize);
        }
    }
}
