using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Implemented by all <see cref="ITikEntity"/> classes, that contains .id property.
    /// Is used for save access in entity list.
    /// </summary>
    /// <seealso cref="IEditableTikList"/>
    public interface ITikEntityWithId: ITikEntity
    {
        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <value>The id value.</value>
        string Id { get; }
    }
}
