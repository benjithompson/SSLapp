using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class GetToscaAppsBehavior : IGetToscaApps
    {
        public IEnumerable<string> GetToscaApps(string appPath)
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
