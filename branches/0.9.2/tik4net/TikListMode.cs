using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Mode of tik list (list of <see cref="ITikEntity"/>.
    /// </summary>
    public enum TikListMode
    {
        /// <summary>
        /// List supports order - items could be moved up/down ...
        /// </summary>
        Ordered,
        /// <summary>
        /// List is unordered - moving items doesn't make sense.
        /// </summary>
        NotOrdered,
        /// <summary>
        /// List contains only one row (maximally). Is typical for objets like /system/resource etc.
        /// </summary>
        SingleRow,
    }
}
