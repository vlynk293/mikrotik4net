using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tik4Net.Connector
{
    /// <summary>
    /// Any exception from mikrotik session.
    /// </summary>
    [Serializable]
    public class TikConnectorException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TikConnectorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        public TikConnectorException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TikConnectorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public TikConnectorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The message (with format params).</param>
        /// <param name="args">The args (for message formating).</param>
        public TikConnectorException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="command">The command sent to target.</param>
        public TikConnectorException(string message, string command)
            : this(FormatMessage(message, command, null, null, null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="command">The command sent to target.</param>
        /// <param name="response">The response from target.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public TikConnectorException(string message, string command, List<string> response)
            : this(FormatMessage(message, command, null, response, null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="command">The command sent to target.</param>
        /// <param name="errors">The list of errors from target.</param>
        /// <param name="response">The response from target.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public TikConnectorException(string message, string command, List<string> errors, List<string> response)
            : this (FormatMessage(message, command, errors, response, null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikConnectorException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="command">The command sent to target.</param>
        /// <param name="formatRegex">The requested format regex that is not matched in response.</param>
        /// <param name="response">The response from target.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public TikConnectorException(string message, string command, Regex formatRegex, List<string> response)
            : this(FormatMessage(message, command, null, response, formatRegex))
        {
        }
        
        private static string FormatMessage(string message, string command, 
            List<string> errors, List<string> response, Regex formatRegex)
        {
            Guard.ArgumentNotNull(message, "message");
            StringBuilder result = new StringBuilder();
            result.AppendLine(message);
            if (!string.IsNullOrEmpty(command))
            {
                result.AppendLine("  COMMAND:");
                foreach (string str in command.Split(new string[] {"\n" }, StringSplitOptions.None))
                {
                    result.AppendLine("    " + str);
                }
            }

            if (errors != null)
            {
                result.AppendLine("  ERRORS:");
                foreach (string str in errors)
                {
                    result.AppendLine("    " + str);
                }
            }

            if (response != null)
            {
                result.AppendLine("  RESPONSE:");
                foreach (string str in response)
                {
                    result.AppendLine("    " + str);
                }
            }

            if (formatRegex != null)
            {
                result.AppendLine(string.Format(CultureInfo.CurrentCulture, "  EXPECTED FORMAT: {0}", formatRegex.ToString()));
            }

            return result.ToString();
        }

    }
}
