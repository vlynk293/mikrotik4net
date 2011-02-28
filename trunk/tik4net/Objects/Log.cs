using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;
using System.Globalization;

namespace Tik4Net.Objects
{   
    /// <summary>
    /// Represents one row in /log on mikrotik router.
    /// </summary>
    public sealed partial class Log: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /log on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class LogList //: TikUnorderedList<Log>
    {
        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogDebug(string message)
        {
            TikSession.CastActiveConnector<ILogConnector>().Log(message, LogLevel.Debug);
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogDebug(string format, params object[] args)
        {
            LogDebug(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogInfo(string message)
        {
            TikSession.CastActiveConnector<ILogConnector>().Log(message, LogLevel.Info);
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogInfo(string format, params object[] args)
        {
            LogInfo(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogWarning(string message)
        {
            TikSession.CastActiveConnector<ILogConnector>().Log(message, LogLevel.Warning);
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogWarning(string format, params object[] args)
        {
            LogWarning(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogError(string message)
        {
            TikSession.CastActiveConnector<ILogConnector>().Log(message, LogLevel.Error);
        }

        /// <summary>
        /// See <see cref="ILogConnector.Log"/> for details.
        /// </summary>
        public static void LogError(string format, params object[] args)
        {
            LogError(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        //TODO Custom LOAD methods to match filtering needs
    }           
}