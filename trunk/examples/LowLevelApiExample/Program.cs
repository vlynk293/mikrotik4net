using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net;
using Tik4Net.Connector.Api;

namespace LowLevelApiExample
{
    /* =LowLevel API example=
     * Main idea is to cast universal ITikConnector to more specific IApiConnector
     * 
     * REMARKS: of course works only for TikConnectorType.Api connector.
     * 
     * REMARKS: these examples are only examples - usable functionality should be implemented directly 
     * on tik-list objects as static methods (the same way as logging).
     */
    class Program
    {
        static string HOST = "192.168.88.1";
        static string USER = "admin";
        static string PASS = "";

        static void Main(string[] args)
        {
            using (TikSession session = new TikSession(TikConnectorType.Api))
            {
                session.Open(HOST, USER, PASS);
                IApiConnector apiConnector = (IApiConnector)session.Connector;

                //ip-scan example
                List<ITikEntityRow> ipScanList = apiConnector.ApiExecuteReader("/tool/ip-scan", new Dictionary<string, string>
                    {
                        {"address-range", "192.168.0.0/16"},
                        {"interface", "ether1"},
                        {"duration", "120" }
                    });
                foreach (ITikEntityRow row in ipScanList)
                {
                    Console.WriteLine("Address: {0}, MAC: {1}, DNS: {2}",
                        row.GetStringValueOrNull("address", true),
                        row.GetStringValueOrNull("mac-address", false),
                        row.GetStringValueOrNull("dns", false));
                }

                //mac-scan sample
                List<ITikEntityRow> macScanList = apiConnector.ApiExecuteReader("/tool/mac-scan", new Dictionary<string, string>
                    {
                        {"interface", "ether1" },
                        {"duration", "120" }
                    });
                foreach (ITikEntityRow row in macScanList)
                {
                    Console.WriteLine("Address: {0}, MAC: {1}",
                        row.GetStringValueOrNull("address", true),
                        row.GetStringValueOrNull("mac-address", true));
                }

                //logging example
                apiConnector.Log("Test", Tik4Net.Objects.LogLevel.Info); //REMARKS - you should use LogList.LogDebug for this case.

                //execute scalar - select for single value
                string result = apiConnector.ApiExecuteScalar("/ip/firewall/connection/print", new Dictionary<string,string>{{"count-only", ""}});
                Console.WriteLine(result);
            }
        }
    }
}
