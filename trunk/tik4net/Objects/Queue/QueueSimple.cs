using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Queue
{   
    /// <summary>
    /// Represents one row in /queue/simple on mikrotik router.
    /// </summary>
    public sealed partial class QueueSimple: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /queue/simple on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class QueueSimpleList //: TikUnorderedList<QueueSimple>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}