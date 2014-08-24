using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.System
{   
    /// <summary>
    /// Represents one row in /system/resource on mikrotik router.
    /// </summary>
    public sealed partial class SystemResource: TikEntityBase
    {
        /// <summary>
        /// Loads the single instance of <see cref="SystemResource"/> (router description and state).
        /// </summary>
        /// <returns>Single instance of <see cref="SystemResource"/> (router description and state)</returns>
        public static SystemResource LoadInstance()
        {
            SystemResourceList list = new SystemResourceList();
            list.LoadAll();
            return list.Single();
        }
    }

    /// <summary>
    /// Represents list of rows in /system/resource on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class SystemResourceList //: TikSingleRowList<SystemResource>
    {
        /// <summary>
        /// See <see cref="SystemResource.LoadInstance"/> for details.
        /// </summary>
        public static SystemResource LoadInstance()
        {
            return SystemResource.LoadInstance();
        }
    }           
}