using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tik4Net.Session
{
    /// <summary>
    /// Exception when parsing mikrotik response rows.
    /// </summary>
    [Serializable]
    public class TikResponseParseException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TikResponseParseException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TikResponseParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikResponseParseException"/> class.
        /// </summary>
        public TikResponseParseException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikResponseParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TikResponseParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikResponseParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public TikResponseParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikResponseParseException"/> class.
        /// </summary>
        /// <param name="message">The message (with format params).</param>
        /// <param name="args">The args (for message formating).</param>
        public TikResponseParseException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
