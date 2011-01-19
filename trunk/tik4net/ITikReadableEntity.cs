using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Interface implemented by all entity objects that are able
    /// to load itself from <see cref="ITikEntityRow"/>. See <see cref="LoadFromEntityRow"/>.
    /// </summary>
    public interface ITikReadableEntity
    {
        /// <summary>
        /// Gets the load path in API format (/queue/tree).
        /// </summary>
        /// <value>Load path for engine.</value>
        string LoadPath { get; }


        /// <summary>
        /// Loads entity state from given data row.
        /// </summary>
        /// <param name="entityRow">The entity data row.</param>
        void LoadFromEntityRow(ITikEntityRow entityRow);
    }
}
