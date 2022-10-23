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
        /// Shape of the minimap mask.
        /// </summary>
        [SerializeField]
        [Tooltip("Shape of the minimap mask.")]
        private Sprite MaskShape;

        /// <summary>
        /// Source of bounds for the minimap.
        /// </summary>
        private IBoundsSource source;

        /// <summary>
        /// Collection of icons contained in this minimap.
        /// </summary>
        private Dictionary<IMinimapIcon, GameObject> icons = new Dictionary<IMinimapIcon, GameObject>();

        /// <summary>
        /// Gets the world bounds for this simple minimap.
        /// </summary>
        private Bounds WorldBounds => source?.GetBounds() ?? defaultBounds;

        public void LateUpdate()
        {
            foreach (IMinimapIcon icon in icons.Keys)
            {
                UpdateMinimapIconPosition(icon);
            }
        }

        public void Start()
        {
            foreach (IMinimapIcon icon in GameObject.FindObjectsOfType<SpriteIcon>())
            {
                AddIcon(icon);
            }
        }

        public void Awake()
        {
            Mask mask = gameObject.AddComponent<Mask>();
            Image maskImage = gameObject.AddComponent<Image>();
            if (MaskShape != null)
            {
                maskImage.sprite = MaskShape;
            }

            var background = new GameObject();
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

        /// <inheritdoc/>
        public bool AddIcon(IMinimapIcon minimapIcon)
        {
            if (icons.ContainsKey(minimapIcon))
            {
                return false;
            }

            GameObject iconGo = minimapIcon.CreateIcon(GetComponent<RectTransform>());
            iconGo.transform.SetParent(transform);

            icons.Add(minimapIcon, iconGo);
            UpdateMinimapIconPosition(minimapIcon);
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
            source ??= GameObject.FindObjectOfType<BoxBoundsSource>() as IBoundsSource;
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

        /// <summary>
        /// UPdate the position of a minimap icon based on its current position.
        /// </summary>
        /// <param name="icon">icon to update position of.</param>
        private void UpdateMinimapIconPosition(IMinimapIcon icon)
        {
            if (icons.TryGetValue(icon, out GameObject iconGo))
            {
                Vector3 relativePosition = GetMinimapPosition(icon.GetWorldSpace());
                RectTransform rectTransform = iconGo.GetComponent<RectTransform>();
                rectTransform.anchorMax = relativePosition;
                rectTransform.anchorMin = relativePosition;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.rotation = Quaternion.Euler(0, 0, -icon.GetIconRotation().eulerAngles.y);
            }
        }

        /// <summary>
        /// Translates a position from world space to normalized minimap space.
        /// </summary>
        /// <param name="worldPosition">Position of the object in world space.</param>
        /// <returns>Normalized minimap position, will scale positions within
        /// the minimap to between (0,0) and (1,1).</returns>
        private Vector2 GetMinimapPosition(Vector3 worldPosition)
        {
            Vector3 relativePosition = worldPosition - WorldBounds.min;
            var normalizedPosition = new Vector2(relativePosition.x / WorldBounds.size.x, relativePosition.z / WorldBounds.size.z);
            return normalizedPosition;
        }
    }
}
