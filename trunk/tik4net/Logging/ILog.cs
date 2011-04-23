using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Logging
{
    /// <summary>
    /// Logger object 
    /// </summary>
    /// <seealso cref="ILogFactory"/>
    public interface ILog
    {
        /// <summary>
        /// Gets a value indicating whether this instance allows debug logging.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance allows debug logging; otherwise, <c>false</c>.
        /// </value>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance allows info logging.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance allows info logging; otherwise, <c>false</c>.
        /// </value>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance allows warning logging.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance allows warning logging; otherwise, <c>false</c>.
        /// </value>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance allows error logging.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance allows error logging; otherwise, <c>false</c>.
        /// </value>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Writes the debug message.
        /// </summary>
        void Debug(object message);

        /// <summary>
        /// Writes the debug formated message.
        /// </summary>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Writes the info message.
        /// </summary>
        void Info(object message);

        /// <summary>
        /// Writes the info formated message.
        /// </summary>        
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Writes the warning message.
        /// </summary>
        void Warn(object message);

        /// <summary>
        /// Writes the warning formated message.
        /// </summary>        
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Writes the error message.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error(object message);

        /// <summary>
        /// Writes the error message with exception.
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error(object message, Exception exception);

        /// <summary>
        /// Writes the error formated message.
        /// </summary>
        void ErrorFormat(string format, params object[] args);
    }
}
