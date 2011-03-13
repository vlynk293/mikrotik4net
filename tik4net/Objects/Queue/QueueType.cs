using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Queue
{   
    /// <summary>
    /// Represents one row in /queue/type on mikrotik router.
    /// </summary>
    public sealed partial class QueueType: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /queue/type on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class QueueTypeList //: TikUnorderedList<QueueType>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}