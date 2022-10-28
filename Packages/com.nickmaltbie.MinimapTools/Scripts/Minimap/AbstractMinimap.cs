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
using System.Linq;
using nickmaltbie.MinimapTools.Background;
using nickmaltbie.MinimapTools.Icon;
using nickmaltbie.MinimapTools.Minimap.Shape;
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.MinimapTools.Minimap
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
        /// Number of pixels per unit in the background map.
        /// </summary>
        [SerializeField]
        [Tooltip("Number of pixels per unit in the background map.")]
        internal float pixelsPerUnit = 10;

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

        /// <inheritdoc/>
        public float PixelsPerUnit => pixelsPerUnit;

        /// <summary>
        /// Background texture to render elements onto.
        /// </summary>
        private BackgroundTexture backgroundTexture;

        /// <summary>
        /// Scale factor for map on screen.
        /// </summary>
        public abstract float MapScale { get; }

        /// <summary>
        /// Move each object following minimap rules.
        /// </summary>
        public virtual void LateUpdate()
        {
            backgroundRt.localPosition = -MapOffset * backgroundRt.sizeDelta * MapScale;
            backgroundRt.localScale = Vector3.one * MapScale;

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

            // Generate background image from scene elements
            backgroundTexture = new BackgroundTexture(
                this,
                GetSize(),
                backgroundImage);
            Texture2D tex = backgroundTexture.GetTexture2D();

            Image image = background.AddComponent<Image>();
            image.sprite = Sprite.Create(
                tex,
                new Rect(0.0f, 0.0f, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100);
            RectTransform rt = image.GetComponent<RectTransform>();
            rt.sizeDelta = GetSize();
            rt.localScale = Vector3.one;
        }

        /// <summary>
        /// Add any icons from the minimap to this object during the start.
        /// </summary>
        public virtual void Start()
        {
            foreach (
                AbstractMinimapElement minimapElement in
                GameObject.FindObjectsOfType<AbstractMinimapElement>()
                    .OrderBy(e => e.GetOrder()))
            {
                backgroundTexture.AddElementToMinimap(minimapElement);
            }
        }

        /// <inheritdoc/>
        public virtual bool AddIcon(IMinimapIcon minimapIcon)
        {
            if (icons.ContainsKey(minimapIcon))
            {
                return false;
            }

            GameObject iconGo = minimapIcon.CreateIcon(this, backgroundRt);
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

                if (icon.RotateWithMap())
                {
                    rectTransform.localRotation = Quaternion.Euler(0, 0, -(icon.GetIconRotation().eulerAngles.y + GetRotation()));
                }
                else
                {
                    rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                }

                if (icon.ScaleWithMap())
                {
                    rectTransform.localScale = Vector3.one;
                }
                else
                {
                    rectTransform.localScale = Vector3.one / MapScale;
                }
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

        /// <inheritdoc/>
        public Vector2Int GetSize()
        {
            Vector2 minimapSize = MinimapBounds.Size * pixelsPerUnit;
            return new Vector2Int(Mathf.RoundToInt(minimapSize.x), Mathf.RoundToInt(minimapSize.y));
        }

        /// <inheritdoc/>
        public float GetRotation()
        {
            return MinimapBounds.rotation;
        }
    }
}
