using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Objects.Ip
{   
    /// <summary>
    /// Represents one row in /ip/address on mikrotik router.
    /// </summary>
    public sealed partial class IpAddress: TikEntityBase
    {
        
    }

    /// <summary>
    /// Represents list of rows in /ip/address on mikrotik router - CUSTOM CODE.
    /// </summary>    
    public sealed partial class IpAddressList //: TikUnorderedList<IpAddress>
    {
		//TODO Custom LOAD methods to match filtering needs
    }           
}