using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;

namespace Tik4Net
{
    /// <summary>
    /// Type of connector (<see cref="ITikConnector"/>) used to access mikrotik (predefined or custom)
    /// </summary>
    /// <seealso cref="ITikConnector"/>
    public enum TikConnectorType
    {        
        /// <summary>
        /// Uses Mikrotik API.
        /// </summary>
        Api,
        /// <summary>
        /// Uses SSH connection.
        /// </summary>
        Ssh,
        /// <summary>
        /// Uses Telnet connection.
        /// </summary>
        Telnet,
        /// <summary>
        /// Custom - you have to provide instance of your own <see cref="ITikConnector"/> interface implementation.
        /// </summary>
        Custom,
    }
}
