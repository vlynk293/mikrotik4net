using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net;
using Tik4Net.Objects.Ip.Firewall;

namespace MergeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TikSession session = new TikSession(TikConnectorType.Api))
            {
                FirewallMangleList dest = CreateList();
                FirewallMangleList srcTmp = CreateList();

                List<FirewallMangle> src = new List<FirewallMangle>(srcTmp);
                FirewallMangle additionalMangle = new FirewallMangle() { SrcAddress = "192.168.1.4", Action = "allow" };                               
                src.Insert(0, additionalMangle);

                dest.MergeSubset(dest, src, dest.First(), m => m.SrcAddress, 
                    (d, s) => d.Action = s.Action);


                foreach (FirewallMangle mangle in dest)
                {
                    Console.WriteLine(mangle);
                }
                Console.WriteLine("I/U/D/M {0}/{1}/{2}/{3}", dest.NewCount, dest.UpdatedCount, dest.DeletedCount, dest.MovesCount);
                Console.ReadLine();
            }
        }

        private static FirewallMangleList CreateList()
        {
             FirewallMangleList result = new FirewallMangleList();

            for (int i = 1; i <= 3; i++)
            {
                string address = string.Format("192.168.1.{0}", i);
                FirewallMangle mangle = new FirewallMangle() { SrcAddress = address, Action = "allow" };
                result.Add(mangle);

                mangle.MarkClear(); //workaround
            }

            result.ClearMoves();
           
            return result;
        }
    }
}
