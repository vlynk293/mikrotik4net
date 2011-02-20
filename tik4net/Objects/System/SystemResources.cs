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
        
    }

    /// <summary>
    /// Represents list of rows in /system/resource on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class SystemResourceList //: TikSingleRowList<SystemResource>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}