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
        public string minimapBoundsTag;

        /// <summary>
        /// Name of the tag to search for the follow target.
        /// </summary>
        [SerializeField]
        [Tooltip("Name of the tag to search for the follow target.")]
        public string followTargetTag;

        /// <summary>
        /// Should the minimap rotate with the follow target.
        /// </summary>
        [SerializeField]
        [Tooltip("Should the minimap rotate with the follow target.")]
        public bool rotateMinimap = false;

        /// <summary>
        /// Radius of the minimap in world units.
        /// </summary>
        [SerializeField]
        [Tooltip("Radius of the minimap in world units.")]
        public float mapRadius = 10;

        /// <summary>
        /// Follow target for the minimap to center on.
        /// </summary>
        internal IMinimapIcon followTarget;

        /// <summary>
        /// Bounds source for the minimap.
        /// </summary>
        internal MinimapBoundsSource boundsSource;

        /// <summary>
        /// Map offset for following the target.
        /// </summary>
        private Vector2 _mapOffset;

        /// <inheritdoc/>
        protected override MinimapBoundsSource Source => boundsSource;

        /// <inheritdoc/>
        public override Vector2 MapOffset => _mapOffset;

        /// <summary>
        /// Setup initial configuration of minimap.
        /// </summary>
        public override void Awake()
        {
            boundsSource = string.IsNullOrEmpty(minimapBoundsTag) ? null : GameObject.FindGameObjectWithTag(minimapBoundsTag)?.GetComponent<MinimapBoundsSource>();
            followTarget = string.IsNullOrEmpty(followTargetTag) ? null : GameObject.FindGameObjectWithTag(followTargetTag)?.GetComponent<IMinimapIcon>();

            base.Awake();
        }

        public override float MapScale
        {
            get
            {
                RectTransform rt = GetComponent<RectTransform>();
                Vector2 worldSize = GetWorldBounds().Size;
                float mapScale = Mathf.Min(
                    rt.sizeDelta.x / backgroundRt.sizeDelta.x * worldSize.x,
                    rt.sizeDelta.y / backgroundRt.sizeDelta.y * worldSize.y);
                return mapScale / mapRadius / 2;
            }
        }

        public override void Start()
        {
            if (followTarget != null)
            {
                base.AddIcon(followTarget);
            }

            foreach (IMinimapIcon icon in GameObject.FindObjectsOfType<AbstractSpriteIcon>())
            {
                base.AddIcon(icon);
            }

            base.Start();
        }

        /// <inheritdoc/>
        public override void LateUpdate()
        {
            _mapOffset = base.GetMinimapPosition(followTarget.GetWorldSpace()) - Vector2.one * 0.5f;
            float rotation = rotateMinimap ? followTarget.GetIconRotation().eulerAngles.y + base.GetRotation() : 0;
            transform.rotation = Quaternion.Euler(0, 0, rotation);

            base.LateUpdate();
        }
    }
}
