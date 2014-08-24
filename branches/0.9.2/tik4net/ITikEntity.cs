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
    /// <see cref="IChangeTrackingEntity"/>
    public interface ITikEntity
    {
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

        /// <summary>
        /// Assign property values and state flags from given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Source entity (must be identical of type).</param>
        void Assign(object entity);
    }
}
