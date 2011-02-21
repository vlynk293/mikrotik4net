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
    public interface ITikEntity
    {
        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <value>The id value.</value>
        string Id { get; }

        /// <summary>
        /// Loads entity state from given data row.
        /// </summary>
        /// <param name="entityRow">The entity data row.</param>
        void LoadFromEntityRow(ITikEntityRow entityRow);

        /// <summary>
        /// Determines whether is data-equal with the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// 	<c>true</c> if is data-equal with the specified entity; otherwise, <c>false</c>.
        /// </returns>
        bool IsDataEqual(ITikEntity entity);
    }
}
