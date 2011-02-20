using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Queue
{   
    /// <summary>
    /// Represents one row in /queue/tree on mikrotik router.
    /// </summary>
    public sealed partial class QueueTree: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /queue/tree on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class QueueTreeList //: TikList<QueueTree>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}