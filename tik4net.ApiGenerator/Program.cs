using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Tik4Net.ApiGenerator
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                AttachConsole(ATTACH_PARENT_PROCESS);
                ///"v:\Projekty.Mikrotik\tik4net\trunk\tools\defs\Log.xml" "v:\Projekty.Mikrotik\tik4net\trunk\tik4net\Log"
                try
                {
                    string entitySourceXml = args[0];
                    string destinationFile = args[1];

                    Console.WriteLine("Processing {0} -> {1}", entitySourceXml, destinationFile);
                    TikSourceGenerator generator = new TikSourceGenerator(entitySourceXml);
                    string designerSource;
                    string customSource;
                    generator.GenerateSource(out designerSource, out customSource);

                    string designerFileName = destinationFile + ".Designer.cs";
                    Directory.CreateDirectory(Path.GetDirectoryName(designerFileName));
                    File.WriteAllText(designerFileName, designerSource);
                    Console.WriteLine("  - designer file '{0}' written.", designerFileName); 

                    string customFileName = destinationFile + ".cs";
                    if (!File.Exists(customFileName))
                        File.WriteAllText(customFileName, customSource);
                    else
                        Console.WriteLine("  - custom file '{0}' already exist.", customFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("ERROR - Press ENTER");
                    Console.ReadLine();
                }
            }
        }
    }
}
