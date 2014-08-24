using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Logging
{
    internal sealed class DummyLog: ILog
    {
        #region ILog Members

        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false; }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }

        public bool IsErrorEnabled
        {
            get { return false; }
        }

        public void Debug(object message)
        {
            //dummy
        }

        public void DebugFormat(string format, params object[] args)
        {
            //dummy
        }

        public void Info(object message)
        {
            //dummy
        }

        public void InfoFormat(string format, params object[] args)
        {
            //dummy
        }

        public void Warn(object message)
        {
            //dummy
        }

        public void WarnFormat(string format, params object[] args)
        {
            //dummy
        }

        public void Error(object message)
        {
            //dummy
        }

        public void Error(object message, Exception exception)
        {
            //dummy
        }

        public void ErrorFormat(string format, params object[] args)
        {
            //dummy
        }

        #endregion
    }
}
