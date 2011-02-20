using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects
{   
    /// <summary>
    /// Represents one row in /log on mikrotik router.
    /// </summary>
    public sealed partial class Log: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /log on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class LogList //: TikUnorderedList<Log>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}