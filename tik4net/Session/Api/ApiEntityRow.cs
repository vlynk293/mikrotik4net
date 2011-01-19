using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Session.Api
{
    /// <summary>
    /// <see cref="ITikEntityRow"/> that is able to parse datarows in format obtained via Mikrotik API.
    /// <example>
    /// !re=.id=*100004B=name=TOP=parent=global-out=packet-mark==limit-at=10000000=queue=wireless-default=priority=3=max-limit=15000000=burst-limit=0=burst-threshold=0=burst-time=00:00:00=bytes=0=packets=0=dropped=0=rate=0=packet-rate=0=queued-packets=0=queued-bytes=0=lends=0=borrows=0=pcq-queues=0=disabled=true=invalid=true
    /// </example>
    /// </summary>
    public class ApiEntityRow: ITikEntityRow
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
            if (!dataRow.StartsWith("!re=", StringComparison.OrdinalIgnoreCase))
                throw new TikResponseParseException("Invalid response row: {0}", dataRow);

            string data = dataRow.Substring(4);
            string[] items = data.Split('=');
            if ((items.Length % 2) == 1)
                throw new TikResponseParseException("Some value in dataRow contains separator '{0}'. Row: {1}", SEPARATOR, dataRow);
            for (int i = 0; i < (items.Length / 2); i++)
            {
                this.items.Add(items[i * 2], items[i * 2 + 1]);
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
        /// See <see cref="ITikEntityRow.GetValue"/> for details.
        /// </summary>
        public string GetValue(string key)
        {
            string result;
            if (!items.TryGetValue(key, out result))
                throw new TikResponseParseException("DataRow does not contains item with key '{0}'.", key);
            else
                return result;
        }

        /// <summary>
        /// See <see cref="ITikEntityRow.GetValueOrDefault"/> for details.
        /// </summary>
        public string GetValueOrDefault(string key, string defaultValue)
        {
            string result;
            if (!items.TryGetValue(key, out result))
                return defaultValue;
            else
                return result;
        }

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
