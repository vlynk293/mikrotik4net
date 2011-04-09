using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Interfaces
{   
    /// <summary>
    /// Represents one row in /interface/ethernet on mikrotik router.
    /// </summary>
    public sealed partial class InterfaceEthernet: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /interface/ethernet on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class InterfaceEthernetList //: TikUnorderedList<InterfaceEthernet>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}