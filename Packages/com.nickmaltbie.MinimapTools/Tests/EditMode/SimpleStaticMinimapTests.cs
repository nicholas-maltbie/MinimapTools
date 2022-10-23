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
using com.nickmaltbie.MinimapTools.Minimap.MinimapBounds;
using com.nickmaltbie.MinimapTools.Minimap.Simple;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode
{
    /// <summary>
    /// Tests for the <see cref="com.nickmaltbie.MinimapTools.Simple.SimpleStaticMinimap"/>
    /// </summary>
    [TestFixture]
    public class SimpleStaticMinimapTests : TestBase
    {
        private SimpleStaticMinimap simpleMinimap;
        private SpriteIcon spriteIcon;

        [SetUp]
        public void SetUp()
        {
            Debug.Log($"Setting up test: {nameof(SimpleStaticMinimap)}");
            GameObject canvas = base.CreateGameObject();
            canvas.AddComponent<Canvas>();

            GameObject boundsGo = base.CreateGameObject();
            GameObject minimapGo = base.CreateGameObject();
            GameObject iconGo = base.CreateGameObject();

            minimapGo.transform.SetParent(canvas.transform);

            BoxBoundsSource boxBoundsSource = boundsGo.AddComponent<BoxBoundsSource>();
            simpleMinimap = minimapGo.AddComponent<SimpleStaticMinimap>();
            spriteIcon = iconGo.AddComponent<SpriteIcon>();

            simpleMinimap.Awake();
            simpleMinimap.Start();

            simpleMinimap.OnScreenLoaded();
        }

        [TearDown]
        public override void TearDown()
        {
            simpleMinimap.OnScreenUnloaded();
            base.TearDown();
        }

        /// <summary>
        /// Simple sample script test to validate setup and update functions.
        /// </summary>
        [Test]
        public void Validate_SimpleStaticMinimap_LateUpdate()
        {
            Assert.AreEqual(simpleMinimap.transform.childCount, 2);

            simpleMinimap.LateUpdate();
        }

        /// <summary>
        /// Simple sample script test to validate remove function.
        /// </summary>
        [Test]
        public void Validate_SimpleStaticMinimap_RemoveIcon()
        {
            // Remove icon multiple times.
            Assert.AreEqual(simpleMinimap.transform.childCount, 2);
            simpleMinimap.RemoveIcon(spriteIcon);
            Assert.AreEqual(simpleMinimap.transform.childCount, 1);
            simpleMinimap.RemoveIcon(spriteIcon);
            Assert.AreEqual(simpleMinimap.transform.childCount, 1);

            // Add duplicate icons.
            simpleMinimap.AddIcon(spriteIcon);
            Assert.AreEqual(simpleMinimap.transform.childCount, 2);
            simpleMinimap.AddIcon(spriteIcon);
            Assert.AreEqual(simpleMinimap.transform.childCount, 2);
        }

        /// <summary>
        /// Check if an object is in the map.
        /// </summary>
        [Test]
        public void Validate_SimpleStaticMinimap_InMap()
        {
            Assert.IsTrue(simpleMinimap.InMap(Vector3.zero));
            Assert.IsFalse(simpleMinimap.InMap(new Vector3(10, 10, 10)));
        }
    }
}
