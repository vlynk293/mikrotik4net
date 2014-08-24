using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Logging
{
    internal sealed class DummyLogFactory: ILogFactory
    {
        #region ILogFactory Members

        public ILog CreateLogger(Type type)
        {
            return new DummyLog();
        }

        #endregion
    }
}
