using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Ip.Firewall
{   
    /// <summary>
    /// Represents one row in /ip/firewall/filter on mikrotik router.
    /// </summary>
    public sealed partial class FirewallFilter: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /ip/firewall/filter on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class FirewallFilterList //: TikList<FirewallFilter>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}