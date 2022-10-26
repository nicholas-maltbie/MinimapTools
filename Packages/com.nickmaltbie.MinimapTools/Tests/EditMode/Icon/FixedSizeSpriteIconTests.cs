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
using com.nickmaltbie.MinimapTools.Icon;
using com.nickmaltbie.MinimapTools.Utils;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Icon
{
    /// <summary>
    /// Tests for the <see cref="com.nickmaltbie.MinimapTools.Icon.FixedSizeSpriteIcon"/>
    /// </summary>
    [TestFixture]
    public class FixedSizeSpriteIconTests : TestBase
    {
        [Test]
        public void Validate_FixedSizeSpriteIcon(
            [NUnit.Framework.Range(10, 20, 5)] int width,
            [NUnit.Framework.Range(10, 20, 5)] int height
        )
        {
            FixedSizeSpriteIcon icon = CreateGameObject().AddComponent<FixedSizeSpriteIcon>();
            icon.pixelSize = new Vector2Int(width, height);
            Assert.AreEqual(icon.pixelSize, icon.GetPixelSize(null));
            Assert.AreEqual(icon.ScaleWithMap(), false);
            Assert.AreEqual(icon.GetWorldSize(), Vector2.one);
        }
    }
}
