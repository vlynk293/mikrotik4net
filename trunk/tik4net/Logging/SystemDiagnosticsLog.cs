using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace Tik4Net.Logging
{
    internal sealed class SystemDiagnosticsLog: ILog
    {
        private string category;

        internal SystemDiagnosticsLog(Type type)
        {
            category = type != null ? type.ToString() : "";
        }

        #region ILog Members

        public bool IsDebugEnabled
        {
            get { return System.Diagnostics.Debug.Listeners.Count > 0; }
        }

        public bool IsInfoEnabled
        {
            get { return Trace.Listeners.Count > 0; }
        }

        public bool IsWarnEnabled
        {
            get { return Trace.Listeners.Count > 0; }
        }

        public bool IsErrorEnabled
        {
            get { return Trace.Listeners.Count > 0; }
        }

        public void Debug(object message)
        {
            System.Diagnostics.Debug.Write(message, category);
        }

        public void DebugFormat(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args), category);
        }

        private string FormatCategoryAndLevel(string level)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}:{1}", level, category);
        }

        public void Info(object message)
        {
            Trace.WriteLine(message, FormatCategoryAndLevel("INFO"));
        }

        public void InfoFormat(string format, params object[] args)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args), FormatCategoryAndLevel("INFO"));
        }

        public void Warn(object message)
        {
            Trace.WriteLine(message, FormatCategoryAndLevel("WARN"));
        }

        public void WarnFormat(string format, params object[] args)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args), FormatCategoryAndLevel("WARN"));
        }

        public void Error(object message)
        {
            Trace.WriteLine(message, FormatCategoryAndLevel("ERROR"));
        }

        public void Error(object message, Exception exception)
        {
            ErrorFormat("{0}\n{1}", message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args), FormatCategoryAndLevel("ERROR"));
        }

        #endregion
    }
}
