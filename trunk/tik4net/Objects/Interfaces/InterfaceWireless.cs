using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Interfaces
{   
    /// <summary>
    /// Represents one row in /interface/wireless on mikrotik router.
    /// </summary>
    public sealed partial class InterfaceWireless: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /interface/wireless on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class InterfaceWirelessList //: TikUnorderedList<InterfaceWireless>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}