using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// The same as <see cref="TikEntityEditMode"/> but for single property.
    /// <remarks>
    /// If <see cref="TikEntityEditMode"/> of entity is <see cref="TikEntityEditMode.ReadOnly"/>
    /// than <see cref="Editable"/> mode of propperty is impossible.
    /// </remarks>
    /// </summary>
    public enum TikPropertyEditMode
    {
        /// <summary>
        /// Property is editable.
        /// </summary>
        Editable,
        /// <summary>
        /// Property is R/O.
        /// </summary>
        ReadOnly,
    }
}
