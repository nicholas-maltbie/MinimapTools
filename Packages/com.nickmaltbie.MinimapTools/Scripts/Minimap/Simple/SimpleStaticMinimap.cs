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
using com.nickmaltbie.MinimapTools.Minimap.Shape;
using nickmaltbie.ScreenManager;
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Minimap.Simple
{
    /// <summary>
    /// Simple, static minimap that does not move (static) and simply displays
    /// objects on top of a pre-rendered background image. Icons added to the
    /// map will have their position updated each frame to follow the minimap.
    /// </summary>
    public class SimpleStaticMinimap : AbstractMinimap, IScreenComponent
    {
        /// <summary>
        /// Bounds source for the minimap.
        /// </summary>
        private MinimapBoundsSource boundsSource;

        /// <inheritdoc/>
        protected override MinimapBoundsSource Source => boundsSource;

        /// <inheritdoc/>
        public override Vector2 MapOffset => Vector2.zero;

        protected override float MapScale
        {
            get 
            {
                RectTransform rt = GetComponent<RectTransform>();
                return Mathf.Min(rt.sizeDelta.x / backgroundRt.sizeDelta.x, rt.sizeDelta.y / backgroundRt.sizeDelta.y );
            }
        }

        public override void Awake()
        {
            // Update bounds source based on source in scene
            boundsSource ??= GameObject.FindObjectOfType<MinimapBoundsSource>();
            base.Awake();
        }

        /// <inheritdoc/>
        public void OnScreenLoaded()
        {

        }

        /// <inheritdoc/>
        public void OnScreenUnloaded()
        {

        }

        public override void Start()
        {
            foreach (IMinimapIcon icon in GameObject.FindObjectsOfType<AbstractSpriteIcon>())
            {
                AddIcon(icon);
            }

            base.Start();
        }
    }
}
