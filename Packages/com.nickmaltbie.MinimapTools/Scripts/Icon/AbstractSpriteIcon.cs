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

using nickmaltbie.MinimapTools.Minimap;
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.MinimapTools.Icon
{
    /// <summary>
    /// Icon for a minimap composed of a simple sprite.
    /// </summary>
    public abstract class AbstractSpriteIcon : MonoBehaviour, IMinimapIcon
    {
        /// <summary>
        /// Icon for this object on the minimap.
        /// </summary>
        [Tooltip("Icon for this object on the minimap.")]
        public Sprite sprite;

        /// <summary>
        /// Should the icon rotate with the object's rotation.
        /// </summary>
        [Tooltip("Should the icon rotate with the object's rotation")]
        public bool rotateIcon = true;

        /// <summary>
        /// Size of the icon in pixels.
        /// </summary>
        /// <param name="minimap">Minimap the icon will be drawn on.
        /// May affect the size of the icon.</param>
        /// <returns>Size of the icon in pixels.</returns>
        public abstract Vector2Int GetPixelSize(IMinimap minimap);

        /// <inheritdoc/>
        public abstract bool ScaleWithMap();

        /// <inheritdoc/>
        public Vector3 GetWorldSpace() => transform.position;

        /// <inheritdoc/>
        public Quaternion GetIconRotation() => transform.rotation;

        /// <inheritdoc/>
        public bool RotateWithMap() => rotateIcon;

        /// <inheritdoc/>
        public GameObject CreateIcon(IMinimap minimap, RectTransform minimapTransform)
        {
            var go = new GameObject();
            Image image = go.AddComponent<Image>();
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(minimapTransform);

            image.sprite = sprite;
            rt.anchorMin = Vector2.one * 0.5f;
            rt.anchorMax = Vector2.one * 0.5f;
            Vector2Int pixelSize = GetPixelSize(minimap);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelSize.x);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pixelSize.y);
            return go;
        }

        /// <inheritdoc/>
        public virtual Vector2 GetWorldSize() => Vector2.one;
    }
}
