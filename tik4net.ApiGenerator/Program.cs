using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Tik4Net.ApiGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length != 2)
            {
                Application.Run(new Form1());
            }
            else
            {
                string entityPath = args[0];
                string destination = args[1];                
                string srcCode = new Form1().GenerateEntitySouceCodeFromMk(entityPath);

                File.WriteAllText(destination, srcCode);
            }
        }
    }
}
