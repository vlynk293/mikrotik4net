using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net;
using Tik4Net.Objects.System;

namespace SimpleReadOnlyDemo
{
    /* =SimpleReadOnlyDemo=
     * Shows how to read info abou mikrotik router.
     */
    class Program
    {
        static string HOST = "192.198.88.1";
        static string USER = "admin";
        static string PASS = "";

        static void Main(string[] args)
        {
            using (TikSession session = new TikSession(TikConnectorType.Api))
            {
                session.Open(HOST, USER, PASS);

                SystemResource resource = SystemResource.LoadInstance();
                Console.WriteLine("Mikrotik version: {0}", resource.Version);
            }
            Console.ReadLine();
        }
    }
}
