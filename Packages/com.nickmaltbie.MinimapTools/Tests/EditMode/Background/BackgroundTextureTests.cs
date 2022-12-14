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
using nickmaltbie.MinimapTools.Background;
using nickmaltbie.MinimapTools.Minimap;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Background
{
    /// <summary>
    /// Tests for the <see cref="nickmaltbie.MinimapTools.Background.BackgroundTexture"/>
    /// </summary>
    [TestFixture]
    public class BackgroundTextureTests : TestBase
    {
        [Test]
        public void Validate_BackgroundTexture_SetupTexture()
        {
            var minimapMock = new Mock<IMinimap>();
            Texture2D texture = Texture2D.blackTexture;
            var background = new BackgroundTexture(minimapMock.Object, Vector2Int.one * 100, texture);

            int drawCount = 0;
            var minimapElementMock = new Mock<IMinimapElement>();
            minimapElementMock.Setup(e => e.DrawOnBackground(It.IsAny<IMinimap>(), It.IsAny<Texture2D>()))
                .Callback((IMinimap minimap, Texture2D tex) => drawCount++);
            background.AddElementToMinimap(minimapElementMock.Object);
        }

        [Test]
        public void Validate_BackgroundTexture_SetupBGColor()
        {
            var minimapMock = new Mock<IMinimap>();
            var background = new BackgroundTexture(minimapMock.Object, Vector2Int.one * 100, Color.clear);

            int drawCount = 0;
            var minimapElementMock = new Mock<IMinimapElement>();
            minimapElementMock.Setup(e => e.DrawOnBackground(It.IsAny<IMinimap>(), It.IsAny<Texture2D>()))
                .Callback((IMinimap minimap, Texture2D tex) => drawCount++);
            background.AddElementToMinimap(minimapElementMock.Object);
        }
    }
}
