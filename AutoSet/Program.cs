using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
//using System.Runtime.InteropServices;

namespace AutoSet
{
    static class Program
    {
        //[DllImport( "kernel32.dll", SetLastError = true )]
        //[return: MarshalAs( UnmanagedType.Bool )]
        //public static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //AllocConsole();
            Application.Run(new Form1());
        }
    }
}
