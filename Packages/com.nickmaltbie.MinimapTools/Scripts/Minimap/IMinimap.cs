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
using UnityEngine;

namespace com.nickmaltbie.MinimapTools.Minimap
{
    /// <summary>
    /// Minimap for managing state.
    /// </summary>
    public interface IMinimap
    {
        /// <summary>
        /// Check if a location is in the minimap space.
        /// </summary>
        /// <param name="worldSpace">World space of the object.</param>
        /// <returns>True if the location is within the minimap, false otherwise.</returns>
        bool InMap(Vector3 worldSpace);

        /// <summary>
        /// Add an icon to the minimap.
        /// </summary>
        /// <param name="minimapIcon">Icon to add to the minimap.</param>
        /// <returns>True if the object was added successfully, false otherwise.</returns>
        bool AddIcon(IMinimapIcon minimapIcon);

        /// <summary>
        /// Remove an icon from the minimap.
        /// </summary>
        /// <param name="minimapIcon">Icon to remove to the minimap.</param>
        /// <returns>True if the object was removed successfully, false otherwise.</returns>
        bool RemoveIcon(IMinimapIcon minimapIcon);
    }
}
