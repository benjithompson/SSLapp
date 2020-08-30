using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class ToscaInstallation : IGetInstallationDirectories
    {
        public IEnumerable<string> GetAppPathList(string appPath)
        {
            try
            {
                return Directory.GetDirectories(appPath);
            }
            catch (Exception)
            {

                Trace.WriteLine("GetDirectories failed with an exception.");
                return new List<string>();
            }

        }
    }
}
