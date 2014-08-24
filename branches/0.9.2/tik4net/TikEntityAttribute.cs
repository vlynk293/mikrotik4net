using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;

namespace Tik4Net
{
    /// <summary>
    /// Attribute that should decorates <see cref="ITikEntity"/> implementor class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TikEntityAttribute: Attribute
    {
        /// <summary>
        /// Gets the entity path in API notation (/ip/firewall/mangle).
        /// </summary>
        /// <value>The entity path.</value>
        public string EntityPath { get; private set; }

        /// <summary>
        /// Gets the entity edit mode.
        /// </summary>
        /// <value>The edit mode.</value>
        public TikEntityEditMode EditMode { get; private set; }

        /// <summary>
        /// Gets the read flags (modifies ExecuteReader behavior).
        /// </summary>
        /// <value>The read flags (modifies ExecuteReader behavior).</value>
        public ExecuteReaderBehaviors ReaderBehavior { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikEntityAttribute"/> class.
        /// </summary>
        /// <param name="entityPath">The entity path in API notation (/ip/firewall/mangle).</param>
        /// <param name="editMode">The entity edit mode.</param>
        public TikEntityAttribute(string entityPath, TikEntityEditMode editMode)
            : this(entityPath, editMode, Tik4Net.Connector.ExecuteReaderBehaviors.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikEntityAttribute"/> class.
        /// </summary>
        /// <param name="entityPath">The entity path in API notation (/ip/firewall/mangle).</param>
        /// <param name="editMode">The entity edit mode.</param>
        /// <param name="readerBehavior">The reader flags.</param>
        public TikEntityAttribute(string entityPath, TikEntityEditMode editMode, ExecuteReaderBehaviors readerBehavior)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            EntityPath = entityPath;
            EditMode = editMode;
            ReaderBehavior = readerBehavior;
        }
    }
}
