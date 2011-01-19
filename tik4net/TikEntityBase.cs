using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Base class for all strongly-typed Mikrotik entities.
    /// </summary>
    public abstract class TikEntityBase: ITikReadableEntity
    {
        private bool isMarkedDeleted = false;
        private AttributeList attributes = new AttributeList();

        /// <summary>
        /// List of attributes in entity.
        /// </summary>
        protected AttributeList Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modified (any attribute has been modified).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get { return attributes.IsModified; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is marked as deleted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is marked as deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMarkedDeleted 
        {
            get { return isMarkedDeleted; }
        }

        /// <summary>
        /// Gets the id of entity.
        /// </summary>
        /// <value>The entity id.</value>
        public string Id
        {
            get { return attributes.GetAsString(".id"); }
        }

        #region ITikReadableEntity Members

        /// <summary>
        /// See <see cref="ITikReadableEntity.LoadPath"/> for details.
        /// Calls <see cref="GetLoadPath"/>.
        /// </summary>
        public string LoadPath
        {
            get { return GetLoadPath(); }
        }

        /// <summary>
        /// See <see cref="ITikReadableEntity.LoadFromEntityRow"/> for details.
        /// Calls <see cref="OnLoadFromEntityRow"/>.
        /// </summary>
        public void LoadFromEntityRow(ITikEntityRow entityRow)
        {
            attributes.CreateAttribute(".id", entityRow.GetValue(".Id"));

            OnLoadFromEntityRow(entityRow);
        }

        /// <summary>
        /// Called to load entity state (properties) from given <paramref name="entityRow"/>.
        /// </summary>
        /// <param name="entityRow">The entity row.</param>
        protected abstract void OnLoadFromEntityRow(ITikEntityRow entityRow);

        /// <summary>
        /// Retirns load path specific for entity type.
        /// </summary>
        /// <seealso cref="LoadPath"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected abstract string GetLoadPath();

        #endregion

        /// <summary>
        /// Marks this instance as deleted.
        /// </summary>
        public void MarkDeleted()
        {
            isMarkedDeleted = true;
        }

    }
}
