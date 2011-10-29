using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Connector
{
    /// <summary>
    /// Flags that modifies behavior of ExecuteReader method.
    /// </summary>
    [Flags]
    public enum ExecuteReaderBehaviors
    {
        /// <summary>
        /// No modification
        /// </summary>
        None = 0,
        /// <summary>
        /// Exclude 'details' from selection.
        /// </summary>
        ExcludeDetails = 1
    }
}
