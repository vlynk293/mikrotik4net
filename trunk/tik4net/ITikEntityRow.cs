using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Interface implemented by object that represents parsed Mikrotik
    /// data row. Contains list of key-value pairs (row properties).
    /// </summary>
    public interface ITikEntityRow
    {
        /// <summary>
        /// Gets the original (source) data row used to load entity.
        /// </summary>
        /// <value>The data row.</value>
        string DataRow { get; }

        /// <summary>
        /// Gets the keys - all properties read from datarow.
        /// </summary>
        /// <value>The keys.</value>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Determines whether the specified key has been parsed from datarow.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key has been found in datarow; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsItem(string key);

        /// <summary>
        /// Gets the value of specified item from datarow.
        /// </summary>
        /// <param name="key">The key of item (name).</param>
        /// <returns>Value of specified item.</returns>
        /// <remarks>Throws exception if specified <paramref name="key"/> has not been found.</remarks>
        string GetValue(string key);

        /// <summary>
        /// Gets the value of specified item from datarow or <paramref name="defaultValue"/> if key has not been found.
        /// </summary>
        /// <param name="key">The key of item (name).</param>
        /// <param name="defaultValue">The default value - used if <paramref name="key"/> is not found.</param>
        /// <returns>Value of specified item or <paramref name="defaultValue"/>.</returns>
        string GetValueOrDefault(string key, string defaultValue);
    }
}
