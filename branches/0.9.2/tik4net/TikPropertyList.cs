using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Collection of Mikrotik entity properties (attributes).
    /// Supports <see cref="IsModified"/>.
    /// </summary>
    public class TikPropertyList
    {
        private Dictionary<string, TikPropertyItem> items = new Dictionary<string, TikPropertyItem>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets a value indicating whether this instance is modified (any property (attribute) has been modified).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance (any property=attribute) is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get
            {
                foreach (KeyValuePair<string, TikPropertyItem> pair in items)
                {
                    if (pair.Value.IsModified)
                        return true;
                }
                return false;
            }
        }

        private TikPropertyItem GetOrCreateItem(string propertyName)
        {
            TikPropertyItem result;
            if (!items.TryGetValue(propertyName, out result))
            {
                result = new TikPropertyItem();
                items.Add(propertyName, result);
            }

            return result;
        }

        /// <summary>
        /// Sets the property (attribute) value (<see cref="IsModified"/> becomes true).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <param name="value">The value.</param>
        public void SetPropertyValue(string propertyName, string value)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            item.SetValue(value);
        }

        /// <summary>
        /// Sets the attribute value (<see cref="IsModified"/> becomes true).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <param name="value">The value.</param>
        public void SetPropertyValue(string propertyName, long? value)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            item.SetValue(value);
        }

        /// <summary>
        /// Sets the attribute value (<see cref="IsModified"/> becomes true).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <param name="value">The value.</param>
        public void SetPropertyValue(string propertyName, bool? value)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            item.SetValue(value);
        }

        /// <summary>
        /// Creates the new (attribute could not exists before) attribute value 
        /// (<see cref="IsModified"/> of attribute is false).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <param name="value">The value.</param>
        public void CreatePropertyWithValue(string propertyName, string value)
        {
            TikPropertyItem item = new TikPropertyItem(value);
            items.Add(propertyName, item);
        }

        /// <summary>
        /// Creates the new (attribute could not exists before) attribute value 
        /// (<see cref="IsModified"/> of attribute is false).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <param name="value">The value.</param>
        public void CreatePropertyWithValue(string propertyName, bool? value)
        {
            TikPropertyItem item = new TikPropertyItem(value);
            items.Add(propertyName, item);
        }

        /// <summary>
        /// Creates the new (attribute could not exists before) attribute value 
        /// (<see cref="IsModified"/> of attribute is false).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <param name="value">The value.</param>
        public void CreatePropertyWithValue(string propertyName, long? value)
        {
            TikPropertyItem item = new TikPropertyItem(value);
            items.Add(propertyName, item);
        }

        /// <summary>
        /// Gets attribute <paramref name="propertyName"/> as string (value must be set!).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>Property value or exception.</returns>
        /// <exception cref="InvalidOperationException">If property is not defined.</exception>
        public string GetAsString(string propertyName)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            return item.GetAsString();
        }

        /// <summary>
        /// Gets attribute <paramref name="propertyName"/> as bool (or returns 
        /// default value if does not exists).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>Property value or exception.</returns>
        /// <exception cref="InvalidOperationException">If property is not defined.</exception>
        public bool GetAsBoolean(string propertyName)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            return item.GetAsBool();
        }

        /// <summary>
        /// Gets attribute <paramref name="propertyName"/> as Int64 (or returns 
        /// default value if does not exists).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>Property value or exception.</returns>
        /// <exception cref="InvalidOperationException">If property is not defined.</exception>
        public long GetAsInt64(string propertyName)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            return item.GetAsInt64();
        }

        /// <summary>
        /// Gets attribute <paramref name="propertyName"/> as string (or returns 
        /// null if does not exists).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>Property value or null.</returns>
        public string GetAsStringOrNull(string propertyName)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            if (item.HasValue)
                return item.GetAsString();
            else
                return null;
        }

        /// <summary>
        /// Gets attribute <paramref name="propertyName"/> as bool (or returns 
        /// null if does not exists).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>Property value or null.</returns>
        public bool? GetAsBooleanOrNull(string propertyName)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            if (item.HasValue)
                return item.GetAsBool();
            else
                return null;
        }

        /// <summary>
        /// Gets attribute <paramref name="propertyName"/> as Int64 (or returns 
        /// null if does not exists).
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>Property value or null.</returns>
        public long? GetAsInt64OrNull(string propertyName)
        {
            TikPropertyItem item = GetOrCreateItem(propertyName);
            if (item.HasValue)
                return item.GetAsInt64();
            else
                return null;
        }

        /// <summary>
        /// Determines whether contains property with the specified <paramref name="propertyName"/> and property has assigned value.
        /// </summary>
        /// <param name="propertyName">Name of the single property.</param>
        /// <returns>
        /// 	<c>true</c> if contains property with the specified name and it has assigned value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsAssignedProperty(string propertyName)
        {
            TikPropertyItem item;
            if (!items.TryGetValue(propertyName, out item))
                return false;
            else
                return item.HasValue;
        }

        /// <summary>
        /// Gets the state of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="found">if property has been found in list.</param>
        /// <param name="modified">if property value has been modified.</param>
        /// <param name="hasValue">if property has value (is not null).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public void GetPropertyState(string propertyName, out bool found, out bool modified, out bool hasValue)
        {
            TikPropertyItem item;
            if (!items.TryGetValue(propertyName, out item))
            {
                found = false;
                modified = false;
                hasValue = false;
            }
            else
            {
                found = true;
                hasValue = item.HasValue;
                modified = item.IsModified;
            }
        }

        /// <summary>
        /// Determines whether is data-equal with the specified <paramref name="tikPropertyList"/>.
        /// </summary>
        /// <param name="tikPropertyList">The tik property list to compare.</param>
        /// <returns>
        /// 	<c>true</c> if is data-equal with the specified <paramref name="tikPropertyList"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDataEqual(TikPropertyList tikPropertyList)
        {
            foreach (KeyValuePair<string, TikPropertyItem> pair in items)
            {
                TikPropertyItem item2;
                if (!tikPropertyList.items.TryGetValue(pair.Key, out item2))
                    return false;
                else
                {
                    if (!pair.Value.IsDataEqual(item2))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ".id=" + GetOrCreateItem(".id").ToString() + ", " + string.Join(", ",
                items.Where(p => p.Key != ".id" && p.Value.IsModified).OrderBy(p => p.Key).Select(p => p.Key + "=" + p.Value.ToString()).Concat( //modified first
                items.Where(p => p.Key != ".id" && !p.Value.IsModified && p.Value.HasValue).OrderBy(p => p.Key).Select(p => p.Key + "=" + p.Value.ToString())).ToArray()); //and unmodified with values next
        }

        /// <summary>
        /// Marks alle properties as unmodified.
        /// </summary>
        public void MarkClear()
        {
            foreach (KeyValuePair<string, TikPropertyItem> pair in items)
            {
                pair.Value.MarkClear();
            }
        }
    }
}
