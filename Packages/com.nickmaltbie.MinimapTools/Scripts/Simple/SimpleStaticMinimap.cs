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

using System.Collections.Generic;
using nickmaltbie.ScreenManager;
using UnityEngine;
using UnityEngine.UI;

namespace com.nickmaltbie.MinimapTools.Simple
{
    /// <summary>
    /// Simple, static minimap that does not move (static) and simply displays
    /// objects on top of a pre-rendered background image. Icons added to the
    /// map will have their position updated each frame to follow the minimap.
    /// </summary>
    public class SimpleStaticMinimap : MonoBehaviour, IMinimap, IScreenComponent
    {
        /// <summary>
        /// Default bounds for the minimap.
        /// </summary>
        private static readonly Bounds defaultBounds = new Bounds(Vector3.zero, Vector3.one * 10);

        /// <summary>
        /// Background image for the minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Background image for the minimap.")]
        private Sprite backgroundImage;

        /// <summary>
        /// Source of bounds for the minimap.
        /// </summary>
        private IBoundsSource source;

        public void Awake()
        {
            GameObject background = new GameObject();
            background.transform.SetParent(transform);

            RectTransform backgroundTr = background.AddComponent<RectTransform>();
            backgroundTr.anchorMin = Vector2.zero;
            backgroundTr.anchorMax = Vector2.one;
            backgroundTr.pivot = Vector3.zero;
            backgroundTr.sizeDelta = Vector2.one;
            backgroundTr.anchoredPosition = Vector2.zero;

            Image image = background.AddComponent<Image>();
            image.sprite = backgroundImage;
        }

        /// <summary>
        /// Collection of icons contained in this minimap.
        /// </summary>
        private ICollection<IMinimapIcon> icons = new HashSet<IMinimapIcon>();

        /// <summary>
        /// Gets the world bounds for this simple minimap.
        /// </summary>
        private Bounds WorldBounds => source?.GetBounds() ?? defaultBounds;

        /// <summary>
        /// Translates a position from world space to normalized minimap space.
        /// </summary>
        /// <param name="worldPosition">Position of the object in world space.</param>
        /// <returns>Normalized minimap position, will scale positions within
        /// the minimap to between (0,0) and (1,1).</returns>
        private Vector2 GetMinimapPosition(Vector3 worldPosition)
        {
            Vector3 relativePosition = worldPosition - WorldBounds.min;
            Vector2 normalizedPosition = new Vector2(relativePosition.x / WorldBounds.size.x, relativePosition.z / WorldBounds.size.z);
            return normalizedPosition;
        }

        /// <inheritdoc/>
        public bool AddIcon(IMinimapIcon minimapIcon)
        {
            if (icons.Contains(minimapIcon))
            {
                return false;
            }
            icons.Add(minimapIcon);
            return true;
        }

        /// <inheritdoc/>
        public bool InMap(Vector3 worldSpace)
        {
            return WorldBounds.Contains(worldSpace);
        }

        /// <inheritdoc/>
        public void OnScreenLoaded()
        {
            // Update bounds source based on source in scene
            this.source ??= GameObject.FindObjectOfType<BoxBoundsSource>() as IBoundsSource;
        }

        /// <inheritdoc/>
        public void OnScreenUnloaded()
        {
            
        }

        /// <inheritdoc/>
        public bool RemoveIcon(IMinimapIcon minimapIcon)
        {
            return icons.Remove(minimapIcon);
        }
    }
}
