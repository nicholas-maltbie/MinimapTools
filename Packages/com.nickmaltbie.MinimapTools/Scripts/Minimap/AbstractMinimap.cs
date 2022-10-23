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
using com.nickmaltbie.MinimapTools.Icon;
using com.nickmaltbie.MinimapTools.Minimap.MinimapBounds;
using nickmaltbie.ScreenManager;
using UnityEngine;
using UnityEngine.UI;

namespace com.nickmaltbie.MinimapTools.Minimap
{
    /// <summary>
    /// Abstract minimap that contains basic functions for  minimap.
    /// </summary>
    public abstract class AbstractMinimap : MonoBehaviour, IMinimap
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
        protected Sprite backgroundImage;

        /// <summary>
        /// Shape of the minimap mask.
        /// </summary>
        [SerializeField]
        [Tooltip("Shape of the minimap mask.")]
        protected Sprite maskShape;

        /// <summary>
        /// Transform of background image.
        /// </summary>
        protected RectTransform backgroundRt;

        /// <summary>
        /// Collection of icons contained in this minimap.
        /// </summary>
        protected Dictionary<IMinimapIcon, GameObject> icons = new Dictionary<IMinimapIcon, GameObject>();

        /// <summary>
        /// Source of bounds for the minimap.
        /// </summary>
        protected abstract IBoundsSource Source { get; }


        /// <summary>
        /// Scale of the map relative to minimap size.
        /// A value of (1,1) means the minimap is always full size,
        /// A value of (2,2) means the map zoomed in to be twice the size
        /// of the original map.
        /// </summary>
        protected abstract Vector2 MapScale { get; }

        /// <summary>
        /// Map offset for the minimap relative to the map size. A value of (0,0)
        /// would indicate centered map. A value of (1,1) would indicate shift the
        /// minimap by 1 unit of the minimap viewing area.
        /// </summary>
        protected abstract Vector2 MapOffset { get; }

        /// <summary>
        /// Gets the world bounds for this simple minimap.
        /// </summary>
        protected Bounds WorldBounds => Source?.GetBounds() ?? defaultBounds;

        /// <summary>
        /// Move each object following minimap rules.
        /// </summary>
        public virtual void LateUpdate()
        {
            backgroundRt.localPosition = -MapOffset * GetComponent<RectTransform>().sizeDelta;
            backgroundRt.localScale = MapScale;

            foreach (IMinimapIcon icon in icons.Keys)
            {
                UpdateMinimapIconPosition(icon);
            }
        }

        /// <summary>
        /// Initial minimap setup.
        /// </summary>
        public virtual void Awake()
        {
            _ = gameObject.AddComponent<Mask>();
            Image maskImage = gameObject.AddComponent<Image>();
            maskImage.sprite = maskShape;

            var background = new GameObject();
            background.name = "Background";
            background.transform.SetParent(transform);

            backgroundRt = background.AddComponent<RectTransform>();
            backgroundRt.pivot = new Vector2(0.5f, 0.5f);
            backgroundRt.anchorMin = Vector2.zero;
            backgroundRt.anchorMax = Vector2.one;
            backgroundRt.sizeDelta = Vector3.zero;
            backgroundRt.anchoredPosition = Vector2.zero;

            backgroundRt.localScale = MapScale;

            Image image = background.AddComponent<Image>();
            image.sprite = backgroundImage;
        }

        /// <inheritdoc/>
        public virtual bool AddIcon(IMinimapIcon minimapIcon)
        {
            if (icons.ContainsKey(minimapIcon))
            {
                return false;
            }

            GameObject iconGo = minimapIcon.CreateIcon(backgroundRt);
            iconGo.name = "icon";

            icons.Add(minimapIcon, iconGo);
            UpdateMinimapIconPosition(minimapIcon);
            return true;
        }

        /// <inheritdoc/>
        public virtual bool InMap(Vector3 worldSpace)
        {
            return WorldBounds.Contains(worldSpace);
        }

        /// <inheritdoc/>
        public virtual bool RemoveIcon(IMinimapIcon minimapIcon)
        {
            if (icons.TryGetValue(minimapIcon, out GameObject go))
            {
                GameObject.DestroyImmediate(go);
            }

            return icons.Remove(minimapIcon);
        }

        /// <summary>
        /// Update the position of a minimap icon based on its current position.
        /// </summary>
        /// <param name="icon">icon to update position of.</param>
        protected virtual void UpdateMinimapIconPosition(IMinimapIcon icon)
        {
            if (icons.TryGetValue(icon, out GameObject iconGo))
            {
                Vector3 relativePosition = GetMinimapPosition(icon.GetWorldSpace());
                RectTransform rectTransform = iconGo.GetComponent<RectTransform>();
                rectTransform.anchorMax = relativePosition;
                rectTransform.anchorMin = relativePosition;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localRotation = Quaternion.Euler(0, 0, -icon.GetIconRotation().eulerAngles.y);

                iconGo.transform.localScale = new Vector3(1 / MapScale.x, 1 / MapScale.y);
            }
        }

        /// <summary>
        /// Translates a position from world space to normalized minimap space.
        /// </summary>
        /// <param name="worldPosition">Position of the object in world space.</param>
        /// <returns>Normalized minimap position, will scale positions within
        /// the minimap to between (0,0) and (1,1).</returns>
        protected virtual Vector2 GetMinimapPosition(Vector3 worldPosition)
        {
            Vector3 relativePosition = worldPosition - WorldBounds.min;
            var normalizedPosition = new Vector2(relativePosition.x / WorldBounds.size.x, relativePosition.z / WorldBounds.size.z);
            return normalizedPosition;
        }
    }
}
