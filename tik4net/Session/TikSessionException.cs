using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;

namespace Tik4Net.Session
{
    /// <summary>
    /// Any exception from mikrotik session.
    /// </summary>
    [Serializable]
    public class TikSessionException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TikSessionException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TikSessionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSessionException"/> class.
        /// </summary>
        public TikSessionException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSessionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TikSessionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSessionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public TikSessionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSessionException"/> class.
        /// </summary>
        /// <param name="message">The message (with format params).</param>
        /// <param name="args">The args (for message formating).</param>
        public TikSessionException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
