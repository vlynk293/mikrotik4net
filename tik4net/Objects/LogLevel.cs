using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects
{
    /// <summary>
    /// Log level in <see cref="Tik4Net.Connector.ILogConnector.Log"/>.
    /// </summary>
    /// <seealso cref="Tik4Net.Connector.ILogConnector"/>
    public enum LogLevel
    {
        /// <summary>
        /// Debug level.
        /// </summary>
        Debug,
        /// <summary>
        /// Info level.
        /// </summary>
        Info,
        /// <summary>
        /// Warning level.
        /// </summary>
        Warning,
        /// <summary>
        /// Error level.
        /// </summary>
        Error,
    }
}
