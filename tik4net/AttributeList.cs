using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Collection of Mikrotik entity attributes.
    /// Supports <see cref="IsModified"/>.
    /// </summary>
    public class AttributeList
    {
        private Dictionary<string, AttributeItem> items = new Dictionary<string, AttributeItem>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets a value indicating whether this instance is modified (any attribute has been modified).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance (any attribute) is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get
            {
                foreach (KeyValuePair<string, AttributeItem> pair in items)
                {
                    if (pair.Value.IsModified)
                        return true;
                }
                return false;
            }
        }

        private AttributeItem GetOrCreateItem(string attributeName)
        {
            AttributeItem result;
            if (!items.TryGetValue(attributeName, out result))
            {
                result = new AttributeItem();
                items.Add(attributeName, result);
            }

            return result;
        }

        /// <summary>
        /// Sets the attribute value (<see cref="IsModified"/> becomes true).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void SetAttribute(string attributeName, string value)
        {
            AttributeItem item = GetOrCreateItem(attributeName);
            item.SetValue(value);
        }

        /// <summary>
        /// Sets the attribute value (<see cref="IsModified"/> becomes true).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void SetAttribute(string attributeName, long value)
        {
            AttributeItem item = GetOrCreateItem(attributeName);
            item.SetValue(value);
        }

        /// <summary>
        /// Sets the attribute value (<see cref="IsModified"/> becomes true).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void SetAttribute(string attributeName, bool value)
        {
            AttributeItem item = GetOrCreateItem(attributeName);
            item.SetValue(value);
        }

        /// <summary>
        /// Creates the new (attribute could not exists before) attribute value 
        /// (<see cref="IsModified"/> of attribute is false).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void CreateAttribute(string attributeName, string value)
        {
            AttributeItem item = new AttributeItem(value);
            items.Add(attributeName, item);
        }

        /// <summary>
        /// Creates the new (attribute could not exists before) attribute value 
        /// (<see cref="IsModified"/> of attribute is false).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void CreateAttribute(string attributeName, bool value)
        {
            AttributeItem item = new AttributeItem(value);
            items.Add(attributeName, item);
        }

        /// <summary>
        /// Creates the new (attribute could not exists before) attribute value 
        /// (<see cref="IsModified"/> of attribute is false).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void CreateAttribute(string attributeName, long value)
        {
            AttributeItem item = new AttributeItem(value);
            items.Add(attributeName, item);
        }

        /// <summary>
        /// Gets attribute <paramref name="attributeName"/> as string (or returns 
        /// default value if does not exists).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Attribute value or default value.</returns>
        public string GetAsString(string attributeName)
        {
            AttributeItem item = GetOrCreateItem(attributeName);
            return item.GetAsString();
        }

        /// <summary>
        /// Gets attribute <paramref name="attributeName"/> as bool (or returns 
        /// default value if does not exists).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Attribute value or default value.</returns>
        public bool GetAsBoolean(string attributeName)
        {
            AttributeItem item = GetOrCreateItem(attributeName);
            return item.GetAsBool();
        }

        /// <summary>
        /// Gets attribute <paramref name="attributeName"/> as Int64 (or returns 
        /// default value if does not exists).
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Attribute value or default value.</returns>
        public long GetAsInt64(string attributeName)
        {
            AttributeItem item = GetOrCreateItem(attributeName);
            return item.GetAsInt64();
        }

        /// <summary>
        /// Determines whether contains attribute with the specified attribute name and attribute has value.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        /// 	<c>true</c> if contains attribute the specified attribute name and attribute has value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsAttributeWithValue(string attributeName)
        {
            AttributeItem item;
            if (!items.TryGetValue(attributeName, out item))
                return false;
            else
                return item.HasValue;
        }
    }
}
