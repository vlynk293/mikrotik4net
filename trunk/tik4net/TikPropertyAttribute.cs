using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Attribute to mark object property as auto-readable from mikrotik router.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TikPropertyAttribute: Attribute
    {
        /// <summary>
        /// Gets the data type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType { get; private set; }

        /// <summary>
        /// Gets the name of the property (on mikrotik).
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this property is mandatory.
        /// </summary>
        /// <value><c>true</c> if mandatory; otherwise, <c>false</c>.</value>
        public bool Mandatory { get; private set; }

        /// <summary>
        /// Gets the edit mode of property.
        /// </summary>
        /// <value>The edit mode of property.</value>
        public TikPropertyEditMode EditMode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property (on mikrotik).</param>
        /// <param name="propertyType">Data type of the property.</param>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <param name="editMode">The property edit mode.</param>
        public TikPropertyAttribute(string propertyName, Type propertyType, bool mandatory, TikPropertyEditMode editMode)
        {
            Guard.ArgumentNotNull(propertyType, "propertyType");
            Guard.ArgumentNotNullOrEmptyString(propertyName, "propertyName");

            PropertyType = propertyType;
            PropertyName = propertyName;
            Mandatory = mandatory;
            EditMode = editMode;
        }

        //public TikPropertyAttribute(string propertyName, Type propertyType, bool mandatory)
        //    : this(propertyName, propertyType, mandatory, TikPropertyEditMode.Editable)
        //{
        //}


        ///// <summary>
        ///// Initializes a new instance of the <see cref="TikPropertyAttribute"/> class.
        ///// Property is marked as non-mandatory.
        ///// </summary>
        ///// <param name="propertyName">Name of the property (on mikrotik).</param>
        ///// <param name="propertyType">Data type of the property.</param>
        //public TikPropertyAttribute(string propertyName, Type propertyType)
        //    : this(propertyName, propertyType, false)
        //{
        //}
    }
}
