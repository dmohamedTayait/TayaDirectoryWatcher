using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;

namespace TayaIT.DirectoryWatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmNotifier());
            
            
                        ProcessStartInfo psi = new ProcessStartInfo();
            psi.Verb = "runas";
            psi.UseShellExecute = true;


        }

        


    }
}
