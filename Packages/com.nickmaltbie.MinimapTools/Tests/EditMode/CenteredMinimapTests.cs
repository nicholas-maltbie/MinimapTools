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

using com.nickmaltbie.MinimapTools.Icon;
using com.nickmaltbie.MinimapTools.Minimap.Centered;
using com.nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode
{
    /// <summary>
    /// Tests for the <see cref="com.nickmaltbie.MinimapTools.Minimap.Centered.CenteredMinimap"/>
    /// </summary>
    [TestFixture]
    public class CenteredMinimapTests : TestBase
    {
        private CenteredMinimap centeredMinimap;
        private SpriteIcon spriteIcon;
        private GameObject followTarget;

        [SetUp]
        public void SetUp()
        {
            GameObject canvas = base.CreateGameObject();
            canvas.AddComponent<Canvas>();

            GameObject boundsGo = base.CreateGameObject();
            GameObject minimapGo = base.CreateGameObject();
            followTarget = base.CreateGameObject();

            minimapGo.transform.SetParent(canvas.transform);

            MinimapBoundsSource boxBoundsSource = boundsGo.AddComponent<MinimapBoundsSource>();
            centeredMinimap = minimapGo.AddComponent<CenteredMinimap>();

            string followTargetTag = "";
            string minimapBoundsTag = "";

            spriteIcon = followTarget.AddComponent<SpriteIcon>();
            centeredMinimap.followTargetTag = followTargetTag;
            centeredMinimap.minimapBoundsTag = minimapBoundsTag;

            centeredMinimap.Awake();

            centeredMinimap.followTarget = spriteIcon;
            centeredMinimap.boundsSource = boxBoundsSource;

            centeredMinimap.Start();
        }

        /// <summary>
        /// Simple sample script test to validate setup and update functions.
        /// </summary>
        [Test]
        public void Validate_SimpleStaticMinimap_LateUpdate()
        {
            Assert.AreEqual(centeredMinimap.transform.childCount, 1);
            Assert.AreEqual(centeredMinimap.transform.GetChild(0).childCount, 1);

            centeredMinimap.LateUpdate();
        }

        /// <summary>
        /// Simple sample script test to move with the target.
        /// </summary>
        [Test]
        public void Validate_SimpleStaticMinimap_MoveWithTarget([Values(0.5f, 1.0f, 2.0f, 10.0f)] float scale)
        {
            centeredMinimap.mapScale = Vector2.one * scale;

            centeredMinimap.LateUpdate();
            TestUtils.AssertInBounds(centeredMinimap.MapOffset, Vector2.zero);

            followTarget.transform.position = Vector3.forward;
            centeredMinimap.LateUpdate();
            TestUtils.AssertInBounds(centeredMinimap.MapOffset, Vector2.up * scale / 10);

            followTarget.transform.position = Vector3.right;
            centeredMinimap.LateUpdate();
            TestUtils.AssertInBounds(centeredMinimap.MapOffset, Vector2.right * scale / 10);

            followTarget.transform.position = Vector3.up;
            centeredMinimap.LateUpdate();
            TestUtils.AssertInBounds(centeredMinimap.MapOffset, Vector2.zero * scale / 10);
        }
    }
}
