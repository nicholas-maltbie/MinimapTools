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
using com.nickmaltbie.MinimapTools.Background;
using com.nickmaltbie.MinimapTools.Icon;
using com.nickmaltbie.MinimapTools.Minimap.Shape;
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
        public static readonly MinimapSquare defaultShape = new MinimapSquare(Vector3.zero, Vector3.one * 10, 0);

        /// <summary>
        /// Background image for the minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Background image for the minimap.")]
        protected Texture2D backgroundImage;

        /// <summary>
        /// Size of background image in pixels for minimap.
        /// </summary>
        [SerializeField]
        [Tooltip("Size of background image in pixels for minimap.")]
        protected Vector2Int minimapSize = new Vector2Int(1024, 1024);

        /// <summary>
        /// Shape of the minimap mask.
        /// </summary>
        [SerializeField]
        [Tooltip("Shape of the minimap mask.")]
        protected Sprite maskShape;
        
        /// <summary>
        /// Source of shape for the minimap.
        /// </summary>
        protected abstract MinimapBoundsSource Source { get; }

        /// <summary>
        /// Transform of background image.
        /// </summary>
        protected RectTransform backgroundRt;

        /// <summary>
        /// Collection of icons contained in this minimap.
        /// </summary>
        protected Dictionary<IMinimapIcon, GameObject> icons = new Dictionary<IMinimapIcon, GameObject>();

        /// <summary>
        /// Scale of the map relative to minimap size.
        /// A value of (1,1) means the minimap is always full size,
        /// A value of (2,2) means the map zoomed in to be twice the size
        /// of the original map.
        /// </summary>
        public abstract Vector2 MapScale { get; }

        /// <summary>
        /// Map offset for the minimap relative to the map size. A value of (0,0)
        /// would indicate centered map. A value of (1,1) would indicate shift the
        /// minimap by 1 unit of the minimap viewing area.
        /// </summary>
        public abstract Vector2 MapOffset { get; }

        /// <summary>
        /// Get the MinimapSquare version of the minimap bounds.
        /// </summary>
        protected MinimapSquare MinimapBounds => Source?.GetShape() ?? defaultShape;

        /// <inheritdoc/>
        public IMinimapShape GetWorldBounds() => MinimapBounds;

        /// <summary>
        /// Background texture to render elements onto.
        /// </summary>
        private BackgroundTexture backgroundTexture;

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

            // Generate background image from scene elements
            backgroundTexture = new BackgroundTexture(this, minimapSize, backgroundImage);
            Texture2D tex = backgroundTexture.GetTexture2D();

            foreach (AbstractMinimapElement minimapElement in GameObject.FindObjectsOfType<AbstractMinimapElement>())
            {
                backgroundTexture.AddElementToMinimap(minimapElement);
            }

            Image image = background.AddComponent<Image>();
            image.sprite = Sprite.Create(
                tex,
                new Rect(0.0f, 0.0f, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), 100.0f);
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
            return GetWorldBounds().Contains(worldSpace);
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

        /// <inheritdoc/>
        public virtual Vector2 GetMinimapPosition(Vector3 worldPosition)
        {
            float x1, y1;
            MinimapSquare minimapBounds = MinimapBounds;
            (x1, y1) = minimapBounds.GetPositionRelativeToP1(new Vector2(worldPosition.x, worldPosition.z));
            return new Vector2(x1 / minimapBounds.size.x, y1 / minimapBounds.size.y);
        }

        public Vector2Int GetSize() => minimapSize;
    }
}
