using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Tik4Net.Connector.Api
{
    /// <summary>
    /// <see cref="ITikEntityRow"/> that is able to parse datarows in format obtained via Mikrotik API.
    /// <example>
    /// !re=.id=*100004B=name=TOP=parent=global-out=packet-mark==limit-at=10000000=queue=wireless-default=priority=3=max-limit=15000000=burst-limit=0=burst-threshold=0=burst-time=00:00:00=bytes=0=packets=0=dropped=0=rate=0=packet-rate=0=queued-packets=0=queued-bytes=0=lends=0=borrows=0=pcq-queues=0=disabled=true=invalid=true
    /// </example>
    /// </summary>
    internal class ApiEntityRow: ITikEntityRow
    {
        private const string SEPARATOR = "=";
        private Dictionary<string, string> items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEntityRow"/> class.
        /// </summary>
        /// <param name="dataRow">The data row in Mikrotik API format.</param>
        public ApiEntityRow(string dataRow)
        {
            //!re=.id=*100004B=name=TOP=parent=global-out=packet-mark==limit-at=10000000=queue=wireless-default=priority=3=max-limit=15000000=burst-limit=0=burst-threshold=0=burst-time=00:00:00=bytes=0=packets=0=dropped=0=rate=0=packet-rate=0=queued-packets=0=queued-bytes=0=lends=0=borrows=0=pcq-queues=0=disabled=true=invalid=true

            DataRow = dataRow;             
            if (!dataRow.StartsWith("!re", StringComparison.OrdinalIgnoreCase))
                throw new TikResponseParseException("Invalid response row: {0}", dataRow);
            string[] items = dataRow.Split('\n');
            for (int i = 1; i < (items.Length); i++)
            {
                if (!items[i].StartsWith(SEPARATOR, StringComparison.OrdinalIgnoreCase))
                    throw new TikResponseParseException("Some value in dataRow does not start with '{0}'. Row: {1}", SEPARATOR, dataRow);
                string[] parts = items[i].Substring(1).Split(new char[] {'='}, 2);
                if (parts.Length != 2)
                    throw new TikResponseParseException("Invalid response format in datarow '{0}'. Expected '{1}attrName{1}attrValue'", items[i], SEPARATOR);

                this.items.Add(parts[0], parts[1]);
            }
        }


        #region ITikEntityRow Members

        /// <summary>
        /// See <see cref="ITikEntityRow.DataRow"/> for details.
        /// </summary>
        public string DataRow { get; private set; }

        /// <summary>
        /// See <see cref="ITikEntityRow.ContainsItem"/> for details.
        /// </summary>
        public bool ContainsItem(string key)
        {
            return items.ContainsKey(key);
        }

        /// <summary>
        /// See <see cref="ITikEntityRow.GetStringValueOrNull"/> for details.
        /// </summary>
        public string GetStringValueOrNull(string key, bool mandatory)
        {
            string result;
            if (!items.TryGetValue(key, out result))
            {
                if (mandatory)
                    throw new TikResponseParseException("DataRow does not contains item with key '{0}'.", key);
                else
                    return null;
            }
            else
                return result;
        }

        /// <summary>
        /// See <see cref="ITikEntityRow.GetInt64ValueOrNull"/> for details.
        /// </summary>
        public long? GetInt64ValueOrNull(string key, bool mandatory)
        {
            string result;
            if (!items.TryGetValue(key, out result) || string.IsNullOrEmpty(result))
            {
                if (mandatory)
                    throw new TikResponseParseException("DataRow does not contains item with key '{0}'.", key);
                else
                    return null;
            }
            else
            {
                long parsedResult;
                if (!long.TryParse(result, out parsedResult))
                    throw new FormatException(string.Format(CultureInfo.CurrentCulture, "Value '{0}' of property '{1}' can not be parsed to type '{2}'. Row: {3}",
                        result, key, typeof(long), this, DataRow));
                else
                    return parsedResult;
            }
        }

        /// <summary>
        /// See <see cref="ITikEntityRow.GetBoolValueOrNull"/> for details.
        /// </summary>
        public bool? GetBoolValueOrNull(string key, bool mandatory)
        {
            string result;
            if (!items.TryGetValue(key, out result) || string.IsNullOrEmpty(result))
            {
                if (mandatory)
                    throw new TikResponseParseException("DataRow does not contains item with key '{0}'.", key);
                else
                    return null;
            }
            else
            {
                return string.Equals(result, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(result, "yes", StringComparison.OrdinalIgnoreCase);
            }
        }

        ///// <summary>
        ///// See <see cref="ITikEntityRow.GetValueOrDefault"/> for details.
        ///// </summary>
        //public string GetValueOrDefault(string key, string defaultValue)
        //{
        //    string result;
        //    if (!items.TryGetValue(key, out result))
        //        return defaultValue;
        //    else
        //        return result;
        //}

        /// <summary>
        /// See <see cref="ITikEntityRow.Keys"/> for details.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get { return items.Keys; }
        }

        #endregion
    }
}
