using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Ip.Firewall
{   
    /// <summary>
    /// Represents one row in /ip/firewall/address-list on mikrotik router.
    /// </summary>
    public sealed partial class FirewallAddressList: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /ip/firewall/address-list on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class FirewallAddressListList //: TikUnorderedList<FirewallAddressList>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}