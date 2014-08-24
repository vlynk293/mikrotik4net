using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects
{   
    /// <summary>
    /// Represents one row in /interface on mikrotik router.
    /// </summary>
    public sealed partial class Interface: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /interface on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class InterfaceList //: TikUnorderedList<Interface>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}