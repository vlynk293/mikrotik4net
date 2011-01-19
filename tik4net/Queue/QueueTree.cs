using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Queue
{
    /// <summary>
    /// Represents one row in /queue/tree on mikrotik router.
    /// </summary>
    public class QueueTree: TikEntityBase
    {
        /// <summary>
        /// Row borrows property.
        /// </summary>
        public long Borrows 
        { 
            get { return Attributes.GetAsInt64("borrows"); }
            set { Attributes.SetAttribute("borrows", value); }
        }

        /// <summary>
        /// Row burst-limit property.
        /// </summary>
        public long BurstLimit 
        { 
            get { return Attributes.GetAsInt64("burst-limit"); }
            set { Attributes.SetAttribute("burst-limit", value); }
        }        	

        /// <summary>
        /// Row burst-threshold property.
        /// </summary>
        public long BurstThreshold 
        { 
            get { return Attributes.GetAsInt64("burst-threshold"); }
            set { Attributes.SetAttribute("burst-threshold", value); }
        }        	

        /// <summary>
        /// Row burst-time property.
        /// </summary>
        public string BurstTime 
        { 
            get { return Attributes.GetAsString("burst-time"); }
            set { Attributes.SetAttribute("burst-time", value); }
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
        /// Row dropped property.
        /// </summary>
        public long Dropped 
        { 
            get { return Attributes.GetAsInt64("dropped"); }
            set { Attributes.SetAttribute("dropped", value); }
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
        /// Row lends property.
        /// </summary>
        public long Lends 
        { 
            get { return Attributes.GetAsInt64("lends"); }
            set { Attributes.SetAttribute("lends", value); }
        }        	

        /// <summary>
        /// Row limit-at property.
        /// </summary>
        public long LimitAt 
        { 
            get { return Attributes.GetAsInt64("limit-at"); }
            set { Attributes.SetAttribute("limit-at", value); }
        }        	

        /// <summary>
        /// Row max-limit property.
        /// </summary>
        public long MaxLimit 
        { 
            get { return Attributes.GetAsInt64("max-limit"); }
            set { Attributes.SetAttribute("max-limit", value); }
        }        	

        /// <summary>
        /// Row name property.
        /// </summary>
        public string Name 
        { 
            get { return Attributes.GetAsString("name"); }
            set { Attributes.SetAttribute("name", value); }
        }        	

        /// <summary>
        /// Row packet-mark property.
        /// </summary>
        public string PacketMark 
        { 
            get { return Attributes.GetAsString("packet-mark"); }
            set { Attributes.SetAttribute("packet-mark", value); }
        }        	

        /// <summary>
        /// Row packet-rate property.
        /// </summary>
        public long PacketRate 
        { 
            get { return Attributes.GetAsInt64("packet-rate"); }
            set { Attributes.SetAttribute("packet-rate", value); }
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
        /// Row parent property.
        /// </summary>
        public string Parent 
        { 
            get { return Attributes.GetAsString("parent"); }
            set { Attributes.SetAttribute("parent", value); }
        }        	

        /// <summary>
        /// Row pcq-queues property.
        /// </summary>
        public long PcqQueues 
        { 
            get { return Attributes.GetAsInt64("pcq-queues"); }
            set { Attributes.SetAttribute("pcq-queues", value); }
        }        	

        /// <summary>
        /// Row priority property.
        /// </summary>
        public long Priority 
        { 
            get { return Attributes.GetAsInt64("priority"); }
            set { Attributes.SetAttribute("priority", value); }
        }        	

        /// <summary>
        /// Row queue property.
        /// </summary>
        public string Queue 
        { 
            get { return Attributes.GetAsString("queue"); }
            set { Attributes.SetAttribute("queue", value); }
        }        	

        /// <summary>
        /// Row queued-bytes property.
        /// </summary>
        public long QueuedBytes 
        { 
            get { return Attributes.GetAsInt64("queued-bytes"); }
            set { Attributes.SetAttribute("queued-bytes", value); }
        }        	

        /// <summary>
        /// Row queued-packets property.
        /// </summary>
        public long QueuedPackets 
        { 
            get { return Attributes.GetAsInt64("queued-packets"); }
            set { Attributes.SetAttribute("queued-packets", value); }
        }        	

        /// <summary>
        /// Row rate property.
        /// </summary>
        public long Rate 
        { 
            get { return Attributes.GetAsInt64("rate"); }
            set { Attributes.SetAttribute("rate", value); }
        }        	

        /// <summary>
        /// See <see cref="TikEntityBase.GetLoadPath"/> for details.
        /// Returns '/queue/tree' for this type.
        /// </summary>
        protected override string GetLoadPath()
        {
            return "/queue/tree";
        }

        /// <summary>
        /// See <see cref="TikEntityBase.OnLoadFromEntityRow"/> for details.
        /// </summary>
        protected override void OnLoadFromEntityRow(ITikEntityRow entityRow)
        {            
            //!re=.id=*100004C=name=Jenstejn - local=parent=TOP=packet-mark==limit-at=1000000=queue=wireless-default=priority=6=max-limit=8000000=burst-limit=0=burst-threshold=0=burst-time=00:00:00=bytes=0=packets=0=dropped=0=rate=0=packet-rate=0=queued-packets=0=queued-bytes=0=lends=0=borrows=0=pcq-queues=0=disabled=true=invalid=true
                        
            Attributes.CreateAttribute("borrows", long.Parse(entityRow.GetValue("borrows"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("burst-limit", long.Parse(entityRow.GetValue("burst-limit"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("burst-threshold", long.Parse(entityRow.GetValue("burst-threshold"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("burst-time", entityRow.GetValue("burst-time"));
            Attributes.CreateAttribute("bytes", long.Parse(entityRow.GetValue("bytes"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("disabled", string.Equals(entityRow.GetValue("disabled"), "true", StringComparison.OrdinalIgnoreCase));
            Attributes.CreateAttribute("dropped", long.Parse(entityRow.GetValue("dropped"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("invalid", string.Equals(entityRow.GetValue("invalid"), "true", StringComparison.OrdinalIgnoreCase));
            Attributes.CreateAttribute("lends", long.Parse(entityRow.GetValue("lends"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("limit-at", long.Parse(entityRow.GetValue("limit-at"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("max-limit", long.Parse(entityRow.GetValue("max-limit"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("name", entityRow.GetValue("name"));
            Attributes.CreateAttribute("packet-mark", entityRow.GetValue("packet-mark"));
            Attributes.CreateAttribute("packet-rate", long.Parse(entityRow.GetValue("packet-rate"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("packets", long.Parse(entityRow.GetValue("packets"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("parent", entityRow.GetValue("parent"));
            Attributes.CreateAttribute("pcq-queues", long.Parse(entityRow.GetValue("pcq-queues"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("priority", long.Parse(entityRow.GetValue("priority"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("queue", entityRow.GetValue("queue"));
            Attributes.CreateAttribute("queued-bytes", long.Parse(entityRow.GetValue("queued-bytes"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("queued-packets", long.Parse(entityRow.GetValue("queued-packets"), System.Globalization.CultureInfo.CurrentCulture));
            Attributes.CreateAttribute("rate", long.Parse(entityRow.GetValue("rate"), System.Globalization.CultureInfo.CurrentCulture));
        }        
    }
}