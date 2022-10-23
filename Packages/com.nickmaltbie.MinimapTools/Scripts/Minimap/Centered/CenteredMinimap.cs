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
using nickmaltbie.ScreenManager;
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Minimap.Centered
{
    /// <summary>
    /// Centered minimap that follows a labeled object in
    /// the minimap.
    /// </summary>
    public class CenteredMinimap : AbstractMinimap
    {
        /// <summary>
        /// Name of the tag to search for minimap bounds.
        /// </summary>
        [SerializeField]
        [Tooltip("Name of the tag to search for minimap bounds.")]
        private string minimapBoundsTag;

        /// <summary>
        /// Name of the tag to search for the follow target.
        /// </summary>
        [SerializeField]
        [Tooltip("Name of the tag to search for the follow target.")]
        private string followTargetTag;

        /// <summary>
        /// Scale of minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Scale of minimap.")]
        private Vector2 _mapScale = Vector2.one;

        /// <summary>
        /// Follow target for the minimap to center on.
        /// </summary>
        private IMinimapIcon followTarget;

        /// <summary>
        /// Bounds source for the minimap.
        /// </summary>
        private IBoundsSource boundsSource;

        /// <summary>
        /// Map offset for following the target.
        /// </summary>
        private Vector2 _mapOffset;

        /// <inheritdoc/>
        protected override IBoundsSource Source => boundsSource;

        /// <inheritdoc/>
        protected override Vector2 MapOffset => _mapOffset * MapScale - MapScale / 2;

        /// <inheritdoc/>
        protected override Vector2 MapScale => _mapScale;

        /// <summary>
        /// Setup initial configuration of minimap.
        /// </summary>
        public void Start()
        {
            boundsSource ??= GameObject.FindGameObjectWithTag(minimapBoundsTag).GetComponent<IBoundsSource>();
            followTarget ??= GameObject.FindGameObjectWithTag(followTargetTag).GetComponent<IMinimapIcon>();

            if (followTarget != null)
            {
                base.AddIcon(followTarget);
            }
        }

        /// <inheritdoc/>
        public override void LateUpdate()
        {
            _mapOffset = base.GetMinimapPosition(followTarget.GetWorldSpace());

            base.LateUpdate();
        }
    }
}
