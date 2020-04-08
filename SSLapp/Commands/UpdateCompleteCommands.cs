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
            Process iisReset = new Process();
            iisReset.StartInfo.FileName = "iisreset.exe";
            iisReset.StartInfo.RedirectStandardOutput = true;
            iisReset.StartInfo.UseShellExecute = false;
            iisReset.Start();
            iisReset.WaitForExit();
        }

    }
}
