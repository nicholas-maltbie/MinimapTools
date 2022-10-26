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

using com.nickmaltbie.MinimapTools.Background;
using com.nickmaltbie.MinimapTools.Icon;
using com.nickmaltbie.MinimapTools.Minimap;
using com.nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.MinimapTools.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.MinimapTools.Tests.EditMode.Minimap
{
    public class ExampleMinimapElement : AbstractMinimapElement
    {
        private static int totalDraw = 0;
        public int DrawnOrder { get; private set; }
        public int drawCount = 0;

        public override float GetRotation() => 0;

        public override Texture2D GetTexture(IMinimap minimap) => Texture2D.blackTexture;

        public override Vector3 WorldCenter() => Vector3.zero;

        public override void DrawOnBackground(IMinimap minimap, Texture2D backgroundTexture)
        {
            drawCount++;
            DrawnOrder = totalDraw++;
            base.DrawOnBackground(minimap, backgroundTexture);
        }
    }

    public class ExampleMinimap : AbstractMinimap
    {
        public MinimapBoundsSource boundsSource;
        public override Vector2 MapOffset => Vector3.zero;
        public override float MapScale => 1.0f;
        protected override MinimapBoundsSource Source => boundsSource;
    }

    public class ExampleMinimapIcon : AbstractSpriteIcon
    {
        public Vector2Int pixelSize = new Vector2Int(10, 10);
        public bool scaleWithMap;

        public override Vector2Int GetPixelSize(IMinimap minimap) => pixelSize;
        public override bool ScaleWithMap() => scaleWithMap;
    }

    /// <summary>
    /// Tests for the <see cref="com.nickmaltbie.MinimapTools.Minimap.AbstractMinimap"/>
    /// </summary>
    [TestFixture]
    public class AbstractMinimapTests : TestBase
    {
        private ExampleMinimap exampleMinimap;
        private MinimapBoundsSource boundsSource;

        [SetUp]
        public void SetUp()
        {
            GameObject canvas = base.CreateGameObject();
            canvas.AddComponent<Canvas>();
            boundsSource = base.CreateGameObject().AddComponent<MinimapBoundsSource>();
            boundsSource.minimapShape = new MinimapSquare(Vector2.zero, Vector2.one * 10, 0);

            GameObject minimapGo = CreateGameObject();
            minimapGo.transform.SetParent(canvas.transform);
            exampleMinimap = minimapGo.AddComponent<ExampleMinimap>();
            exampleMinimap.boundsSource = boundsSource;
            exampleMinimap.Awake();
        }

        [Test]
        public void Verify_AbstractMinimapDrawElements()
        {
            ExampleMinimapElement example = base.CreateGameObject().AddComponent<ExampleMinimapElement>();
            Assert.AreEqual(example.drawCount, 0);

            exampleMinimap.Start();
            Assert.AreEqual(example.drawCount, 1);
        }

        [Test]
        public void Verify_AbstractMinimap_PixelsPerUnit([Values(10, 20, 5)] float scale)
        {
            exampleMinimap.pixelsPerUnit = scale;
            Assert.AreEqual(exampleMinimap.PixelsPerUnit, scale);
            Assert.AreEqual(exampleMinimap.PixelsPerUnit, exampleMinimap.pixelsPerUnit);
        }

        [Test]
        public void Verify_AbstractMinimap_UninitializedShape()
        {
            Assert.AreEqual(boundsSource.minimapShape, exampleMinimap.GetWorldBounds());

            exampleMinimap.boundsSource = null;
            Assert.AreEqual(AbstractMinimap.defaultShape, exampleMinimap.GetWorldBounds());
        }

        [Test]
        public void Verify_AbstractMinimap_IconPlacement(
            [Values] bool rotateWithMap,
            [Values] bool scaleWithMap)
        {
            GameObject go = CreateGameObject();
            ExampleMinimapIcon icon = go.AddComponent<ExampleMinimapIcon>();
            icon.rotateIcon = rotateWithMap;
            icon.scaleWithMap = scaleWithMap;
            icon.sprite = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.one);

            exampleMinimap.Start();
            exampleMinimap.AddIcon(icon);
            exampleMinimap.LateUpdate();
        }

        [Test]
        public void Verify_AbstractMinimap_DrawOrder(
            [Random(2)] int p1,
            [Random(2)] int p2,
            [Random(2)] int p3
        )
        {
            ExampleMinimapElement e0 = base.CreateGameObject().AddComponent<ExampleMinimapElement>();
            ExampleMinimapElement e1 = base.CreateGameObject().AddComponent<ExampleMinimapElement>();
            ExampleMinimapElement e2 = base.CreateGameObject().AddComponent<ExampleMinimapElement>();

            e0.drawOrder = p1;
            e1.drawOrder = p2;
            e2.drawOrder = p3;

            Assert.AreEqual(e0.drawCount, 0);
            Assert.AreEqual(e1.drawCount, 0);
            Assert.AreEqual(e2.drawCount, 0);

            exampleMinimap.Start();

            Assert.AreEqual(e0.drawCount, 1);
            Assert.AreEqual(e1.drawCount, 1);
            Assert.AreEqual(e2.drawCount, 1);

            // Assert that they were drawn in the order of their priority
            foreach ((ExampleMinimapElement, ExampleMinimapElement) tuple in
                new (ExampleMinimapElement, ExampleMinimapElement)[] { (e0, e1), (e1, e2), (e2, e0) })
            {
                int compare = tuple.Item1.drawOrder.CompareTo(tuple.Item2.drawOrder);
                if (compare != 0)
                {
                    Assert.AreEqual(compare, tuple.Item1.DrawnOrder.CompareTo(tuple.Item2.DrawnOrder));
                }
            }
        }
    }
}
