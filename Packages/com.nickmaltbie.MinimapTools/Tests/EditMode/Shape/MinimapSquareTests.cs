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

using com.nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Shape
{
    /// <summary>
    /// Tests for the <see cref="com.nickmaltbie.MinimapTools.Minimap.Shape.MinimapSquare"/>
    /// </summary>
    [TestFixture]
    public class MinimapSquareTests
    {
        [Test]
        public void Validate_MinimapSquare_Normal()
        {
            var square = new MinimapSquare(Vector2.zero, Vector2.one * 10, 0);

            Assert.AreEqual(square.Center, Vector2.zero);
            Assert.AreEqual(square.Size, Vector2.one * 10);
            Assert.AreEqual(square.Min, Vector2.one * -5);
            Assert.AreEqual(square.Max, Vector2.one * 5);

            TestUtils.AssertInBounds(square.GetPositionRelativeToP1(Vector2.zero), (5, 5));
            TestUtils.AssertInBounds(square.GetPositionRelativeToP1(Vector2.one * -5), (0, 0));
            TestUtils.AssertInBounds(square.GetPositionRelativeToP1(Vector2.one * 5), (10, 10));
            TestUtils.AssertInBounds(square.GetPositionRelativeToP1(new Vector2(-1, 3)), (4, 8));
        }

        [Test]
        public void Validate_MinimapSquare_Rotated()
        {
            var square = new MinimapSquare(Vector2.zero, Vector2.one * 10, 45);

            Assert.AreEqual(square.Center, Vector2.zero);
            Assert.AreEqual(square.Size, Vector2.one * 10);
            TestUtils.AssertInBounds(square.Min, -Vector2.one * 10 * Mathf.Cos(Mathf.PI / 4));
            TestUtils.AssertInBounds(square.Max, Vector2.one * 10 * Mathf.Cos(Mathf.PI / 4));

            TestUtils.AssertInBounds(square.GetPositionRelativeToP1(Vector2.zero), (5, 5));
        }
    }
}
