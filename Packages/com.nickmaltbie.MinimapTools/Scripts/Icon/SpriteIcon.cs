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

using UnityEngine;
using UnityEngine.UI;

namespace com.nickmaltbie.MinimapTools.Icon
{
    /// <summary>
    /// Icon for a minimap composed of a simple sprite.
    /// </summary>
    public class SpriteIcon : MonoBehaviour, IMinimapIcon
    {
        /// <summary>
        /// Icon for this object on the minimap.
        /// </summary>
        [Tooltip("Icon for this object on the minimap.")]
        public Sprite _sprite;

        /// <summary>
        /// Width of the icon in pixels.
        /// </summary>
        [Tooltip("Width of the icon in pixels.")]
        public float width = 10;

        /// <summary>
        /// Height of the icon in pixels.
        /// </summary>
        [Tooltip("Height of the icon in pixels.")]
        public float height = 10;

        /// <summary>
        /// Should the icon rotate with the object's rotation.
        /// </summary>
        [Tooltip("Should the icon rotate with the object's rotation")]
        public bool rotateIcon = false;

        /// <inheritdoc/>
        public Vector3 GetWorldSpace() => transform.position;

        /// <inheritdoc/>
        public Quaternion GetIconRotation() => rotateIcon ? Quaternion.identity : transform.rotation;

        /// <inheritdoc/>
        public GameObject CreateIcon(RectTransform minimapTransform)
        {
            var go = new GameObject();
            Image image = go.AddComponent<Image>();
            image.sprite = _sprite;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(minimapTransform);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            return go;
        }
    }
}
