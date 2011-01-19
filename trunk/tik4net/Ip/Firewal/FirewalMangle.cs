using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Ip.Firewall
{
    /// <summary>
    /// Represents one row in /ip/firewall/mangle on mikrotik router.
    /// </summary>
    public class FirewallMangle: TikEntityBase
    {
        /// <summary>
        /// Row action property.
        /// </summary>
        public string Action 
        { 
            get { return Attributes.GetAsString("action"); }
            set { Attributes.SetAttribute("action", value); }
        }

        /// <summary>
        /// Row bytes property.
        /// </summary>
        public long Bytes 
        { 
            get { return Attributes.GetAsInt64("bytes"); }
            set { Attributes.SetAttribute("bytes", value); }
        }        	

        /// <summary>
        /// Row disabled property.
        /// </summary>
        public bool Disabled 
        { 
            get { return Attributes.GetAsBoolean("disabled"); }
            set { Attributes.SetAttribute("disabled", value); }
        }        	

        /// <summary>
        /// Row dst-port property.
        /// </summary>
        public long DstPort 
        { 
            get { return Attributes.GetAsInt64("dst-port"); }
            set { Attributes.SetAttribute("dst-port", value); }
        }        	

        /// <summary>
        /// Row dynamic property.
        /// </summary>
        public bool Dynamic 
        { 
            get { return Attributes.GetAsBoolean("dynamic"); }
            set { Attributes.SetAttribute("dynamic", value); }
        }        	

        /// <summary>
        /// Row chain property.
        /// </summary>
        public string Chain 
        { 
            get { return Attributes.GetAsString("chain"); }
            set { Attributes.SetAttribute("chain", value); }
        }        	

        /// <summary>
        /// Row invalid property.
        /// </summary>
        public bool Invalid 
        { 
            get { return Attributes.GetAsBoolean("invalid"); }
            set { Attributes.SetAttribute("invalid", value); }
        }        	

        /// <summary>
        /// Row new-connection-mark property.
        /// </summary>
        public string NewConnectionMark 
        { 
            get { return Attributes.GetAsString("new-connection-mark"); }
            set { Attributes.SetAttribute("new-connection-mark", value); }
        }        	

        /// <summary>
        /// Row packets property.
        /// </summary>
        public long Packets 
        { 
            get { return Attributes.GetAsInt64("packets"); }
            set { Attributes.SetAttribute("packets", value); }
        }        	

        /// <summary>
        /// Row passthrough property.
        /// </summary>
        public bool Passthrough 
        { 
            get { return Attributes.GetAsBoolean("passthrough"); }
            set { Attributes.SetAttribute("passthrough", value); }
        }        	

        /// <summary>
        /// Row protocol property.
        /// </summary>
        public string Protocol 
        { 
            get { return Attributes.GetAsString("protocol"); }
            set { Attributes.SetAttribute("protocol", value); }
        }        	

        /// <summary>
        /// Row src-address property.
        /// </summary>
        public string SrcAddress 
        { 
            get { return Attributes.GetAsString("src-address"); }
            set { Attributes.SetAttribute("src-address", value); }
        }        	

        /// <summary>
        /// Row src-address-list property.
        /// </summary>
        public string SrcAddressList 
        { 
            get { return Attributes.GetAsString("src-address-list"); }
            set { Attributes.SetAttribute("src-address-list", value); }
        }        	

        /// <summary>
        /// See <see cref="TikEntityBase.GetLoadPath"/> for details.
        /// Returns '/ip/firewall/mangle' for this type.
        /// </summary>
        protected override string GetLoadPath()
        {
            return "/ip/firewall/mangle";
        }

        /// <summary>
        /// See <see cref="TikEntityBase.OnLoadFromEntityRow"/> for details.
        /// </summary>
        protected override void OnLoadFromEntityRow(ITikEntityRow entityRow)
        {            
            //!re=.id=*6D=chain=prerouting=action=mark-connection=new-connection-mark=NEPLATICI=passthrough=true=protocol=tcp=src-address=10.0.0.0/8=src-address-list=NEPLATICI=dst-port=80=bytes=0=packets=0=disabled=true=invalid=true=dynamic=false
                        
            Attributes.CreateAttribute("action", entityRow.GetValue("action"));
            Attributes.CreateAttribute("bytes", long.Parse(entityRow.GetValue("bytes"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("disabled", string.Equals(entityRow.GetValue("disabled"), "true", StringComparison.OrdinalIgnoreCase));
            Attributes.CreateAttribute("dst-port", long.Parse(entityRow.GetValue("dst-port"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("dynamic", string.Equals(entityRow.GetValue("dynamic"), "true", StringComparison.OrdinalIgnoreCase));
            Attributes.CreateAttribute("chain", entityRow.GetValue("chain"));
            Attributes.CreateAttribute("invalid", string.Equals(entityRow.GetValue("invalid"), "true", StringComparison.OrdinalIgnoreCase));
            Attributes.CreateAttribute("new-connection-mark", entityRow.GetValue("new-connection-mark"));
            Attributes.CreateAttribute("packets", long.Parse(entityRow.GetValue("packets"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("passthrough", string.Equals(entityRow.GetValue("passthrough"), "true", StringComparison.OrdinalIgnoreCase));
            Attributes.CreateAttribute("protocol", entityRow.GetValue("protocol"));
            Attributes.CreateAttribute("src-address", entityRow.GetValue("src-address"));
            Attributes.CreateAttribute("src-address-list", entityRow.GetValue("src-address-list"));
        }        
    }
}