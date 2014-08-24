using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Ip.Firewall
{   
    /// <summary>
    /// Represents one row in /ip/firewall/nat on mikrotik router.
    /// </summary>
    public sealed partial class FirewallNat: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /ip/firewall/nat on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class FirewallNatList //: TikList<FirewallNat>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}