using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net;
using Tik4Net.Objects;
using Tik4Net.Objects.Ip.Firewall;
using System.Text.RegularExpressions;

namespace HackDisabler
{
    /* =HackDisabler=
     * Main idea is to parse log, find dictionary-based atacks in log file and create
     * addresslist items with their ips to allow block them via firewall.
     * 
     * - loads all log error items
     * - if log items is unsuccessfull password-dictionary atack (ssh/ftp)
     *   than disables atacker IP address (adds item to addresslist)
     * - you can easily drop all packets with src-address-list      
     * 
     * REMARKS: in production should be started automatically every 2? minutes (via win. scheduler?)
     */
    class Program
    {
        static string HOST = "192.168.88.1";
        static string USER = "admin";
        static string PASS = "";
        static string DROP_ADDR_LIST = "TO-DROP"; //use this addresslist in firewal-filter to drop packets
        static string IP_PREFIX_WHITELIST = "10.43"; //typycally private network prefix - 
        static List<string> checkedTechnology = new List<string> { "ssh", "telnet", "winbox", "ftp" }; //disable only if login error was via specified technology
        static Regex loginErrorRegex = new Regex(@"^login failure for user (?<USER>\w+) from (?<IP>[\w\.]+) via (?<CONN>\w+)$");
        static int CNT_OF_LOGINS_PER_IP_LIMIT = 3; //if there were more than 3 different user-names with error login (from single IP), than it is hack-atack.
        static int CNT_OF_ERRORS_PER_LOGIN_LIMIT = 1; //if there was were more than 3 unsuccessfull logins with one user-name from single IP - than it is hack-atack.
        static List<string> LOGIN_BLACKLIST = new List<string> { "root","admin" }; //if user name is in blacklist, than previous two lines limit is treated as 1

        static void Main(string[] args)
        {
            using (TikSession session = new TikSession(TikConnectorType.Api))
            {
                session.Open(HOST, USER, PASS);

                //load potential login errors
                LogList logItems = new LogList();
                logItems.LoadByTopics("system,error,critical");

                //load actual addresslist
                FirewallAddressListList addrListItems = new FirewallAddressListList();
                addrListItems.LoadByList(DROP_ADDR_LIST);

                //Find all logon erros (possible atack)
                Dictionary<string, Dictionary<string, int>> atacksPerIp = new Dictionary<string, Dictionary<string, int>>(); //<ip, <user, cnt>>
                foreach (Log log in logItems)
                {
                    Match match = loginErrorRegex.Match(log.Message);
                    if (match.Success) //is logon error format
                    {
                        string atackerIp = match.Groups["IP"].Value;
                        string atackerTechnology = match.Groups["CONN"].Value;
                        string atackerLogin = match.Groups["USER"].Value;
                        if (!atackerIp.StartsWith(IP_PREFIX_WHITELIST, StringComparison.OrdinalIgnoreCase))  //IP is not in white-list
                        {
                            if (checkedTechnology.Contains(atackerTechnology, StringComparer.OrdinalIgnoreCase)) //technology is in checked list (should be handled)
                            {
                                if (!atacksPerIp.ContainsKey(atackerIp))
                                    atacksPerIp.Add(atackerIp, new Dictionary<string, int>());

                                if (!atacksPerIp[atackerIp].ContainsKey(atackerLogin))
                                    atacksPerIp[atackerIp].Add(atackerLogin, 0);

                                atacksPerIp[atackerIp][atackerLogin] = atacksPerIp[atackerIp][atackerLogin] + 1;
                            }
                        }
                    }

                    //Disable all atackers
                    foreach (KeyValuePair<string, Dictionary<string, int>> atackFromIp in atacksPerIp)
                    {
                        if (atackFromIp.Value.Keys.Any(login => LOGIN_BLACKLIST.Contains(login)) //in blacklist
                            || atackFromIp.Value.Keys.Count >= CNT_OF_LOGINS_PER_IP_LIMIT
                            || atackFromIp.Value.Any(p=>p.Value >= CNT_OF_ERRORS_PER_LOGIN_LIMIT))
                        {
                            //should be disabled
                            if (!addrListItems.Any(i => i.Address == atackFromIp.Key))
                            {
                                //not already in disabled list
                                addrListItems.Add(new FirewallAddressList()
                                    {
                                        Address = atackFromIp.Key,
                                        Disabled = false,
                                        List = DROP_ADDR_LIST
                                    });
                            }
                        }
                    }

                    if (addrListItems.IsModified)
                        addrListItems.Save();
                }
            }
        }
    }
}
