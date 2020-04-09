using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SSLapp.ViewModels;
using System.ComponentModel;

using System.Linq;

namespace SSLapp.Commands
{
    class UpdateCompleteCommands
    {
        public static void DoAsyncIISReset(object o, DoWorkEventArgs args)
        {
            Trace.WriteLine("Resetting IIS...");
            using (Process iisReset = new Process())
            {

                iisReset.StartInfo.FileName = "iisreset.exe";
                iisReset.StartInfo.RedirectStandardOutput = false;
                iisReset.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                iisReset.Start();
                iisReset.WaitForExit();
            }
        }
    }
}
