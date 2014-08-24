using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net;

namespace Simple
{
    
    public delegate void EditNewSaved();

    public class PubConst
    {
        private static TikSession tikSessionPrv = null;
        public static TikSession tikSession
        {
            get
            {
                if (tikSessionPrv == null )
                {
                    tikSessionPrv = new TikSession(TikConnectorType.Api);
                }
                return tikSessionPrv;
            }
        }
    }
}
