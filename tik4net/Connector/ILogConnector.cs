using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Objects;

namespace Tik4Net.Connector
{
    /// <summary>
    /// Interface implemented by <see cref="ITikConnector"/> that supports log management via <see cref="Log(string,LogLevel)"/>.
    /// </summary>
    public interface ILogConnector: ITikConnector
    {
        /// <summary>
        /// Writes specified <paramref name="message"/> to log with <paramref name="level"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        void Log(string message, LogLevel level);
    }
}
