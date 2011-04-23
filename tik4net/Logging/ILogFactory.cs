using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Logging
{
    /// <summary>
    /// Log factory (creates instance of <see cref="ILog"/> implementation).
    /// Each instance of tik object that should use logging feature calls <see cref="CreateLogger"/>
    /// and uses given instance of <see cref="ILog"/> to write log items.
    /// </summary>
    /// <seealso cref="ILog"/>
    /// <seealso cref="TikSession.SetLogFactory"/>
    public interface ILogFactory
    {
        /// <summary>
        /// Creates the loger object. Each instance of tik object 
        /// that would write to log creates its own <see cref="ILog"/> logger.
        /// </summary>
        /// <param name="type">The type of tik object.</param>
        /// <returns>Instance of logger object</returns>
        ILog CreateLogger(Type type);
    }
}
