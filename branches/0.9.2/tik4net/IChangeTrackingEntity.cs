using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// All <see cref="ITikEntity"/> classes that supports editation and 
    /// tracking of changes.
    /// </summary>
    /// <see cref="ITikEntity"/>
    public interface IChangeTrackingEntity: ITikEntity
    {
        /// <summary>
        /// Gets a value indicating whether this instance is modified (any attribute has been modified).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        bool IsModified { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is marked as deleted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is marked as deleted; otherwise, <c>false</c>.
        /// </value>
        /// <seealso cref="MarkDeleted"/>
        bool IsMarkedDeleted { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is marked as new.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is marked as new; otherwise, <c>false</c>.
        /// </value>
        /// <seealso cref="MarkNew"/>
        bool IsMarkedNew { get; }

        /// <summary>
        /// Marks this instance as deleted. Flag could be cleared by <see cref="MarkClear"/> call.
        /// </summary>
        void MarkDeleted();

        /// <summary>
        /// Marks this instance as new. Flag could be cleared by <see cref="MarkClear"/> call.
        /// </summary>
        void MarkNew();

        /// <summary>
        /// Clears both flags setup by either <see cref="MarkDeleted"/> or <see cref="MarkNew"/> call.
        /// </summary>
        void MarkClear();

        /// <summary>
        /// Gets all modified properties (for save purposses).
        /// </summary>
        /// <returns>(name,value) list of modified properties.</returns>
        /// <remarks>Performs propertyType-}string conversions on property values.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        Dictionary<string, string> GetAllModifiedProperties();
    }
}
