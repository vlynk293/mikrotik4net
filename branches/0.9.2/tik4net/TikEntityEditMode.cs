using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Mode of <see cref="ITikEntity"/> editation.
    /// </summary>
    /// <seealso cref="TikPropertyEditMode"/>
    public enum TikEntityEditMode
    {
        /// <summary>
        /// Entity is editable (you can use setter of properties).
        /// </summary>
        Editable,
        /// <summary>
        /// Entity is R/O (property setters are not created).
        /// </summary>
        ReadOnly,
    }
}
