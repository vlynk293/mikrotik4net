using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Logging
{
    /// <summary>
    /// Factory creating logger object that uses <see cref="System.Diagnostics.Debug"/>
    /// and <see cref="System.Diagnostics.Trace"/> objects.
    /// </summary>
    public sealed class SystemDiagnosticsLogFactory: ILogFactory
    {        
        #region ILogFactory Members

        /// <summary>
        /// See <see cref="ILogFactory.CreateLogger"/> for details.
        /// </summary>
        public ILog CreateLogger(Type type)
        {
            return new SystemDiagnosticsLog(type);
        }

        #endregion
    }
}
