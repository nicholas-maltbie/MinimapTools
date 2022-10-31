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

using nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Shape
{
    /// <summary>
    /// Tests for the <see cref="nickmaltbie.MinimapTools.Minimap.Shape.MinimapSquare"/>
    /// </summary>
    [TestFixture]
    public class MinimapSquareTests
    {
        [Test]
        public void Validate_MinimapSquare_Normal()
        {
            var square = new MinimapSquare(Vector3.zero, Vector3.one * 10, Quaternion.identity);

            TestUtils.AssertInBounds(square.Center, Vector3.zero);
            TestUtils.AssertInBounds(square.Size, Vector2.one * 10);

            Vector3[] corners = square.GetWorldSpaceCorners(Vector3.zero);
            TestUtils.AssertInBounds(corners[0], new Vector3(-5, 0, -5));
            TestUtils.AssertInBounds(corners[1], new Vector3(-5, 0, 5));
            TestUtils.AssertInBounds(corners[2], new Vector3(5, 0, 5));
            TestUtils.AssertInBounds(corners[3], new Vector3(5, 0, -5));
        }

        [Test]
        public void Validate_MinimapSquare_Rotated(
            [NUnit.Framework.Range(-30, 30, 15)] float rotationX,
            [NUnit.Framework.Range(-30, 30, 15)] float rotationY,
            [NUnit.Framework.Range(-30, 30, 15)] float rotationZ
        )
        {
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
            var square = new MinimapSquare(Vector2.zero, Vector2.one * 10, rotation);

            TestUtils.AssertInBounds(square.Center, Vector3.zero);
            TestUtils.AssertInBounds(square.Size, Vector2.one * 10);

            Vector3[] corners = square.GetWorldSpaceCorners(Vector3.zero);
            TestUtils.AssertInBounds(corners[0], rotation * new Vector3(-5, 0, -5));
            TestUtils.AssertInBounds(corners[1], rotation * new Vector3(-5, 0,  5));
            TestUtils.AssertInBounds(corners[2], rotation * new Vector3( 5, 0,  5));
            TestUtils.AssertInBounds(corners[3], rotation * new Vector3( 5, 0, -5));
        }
    }
}
