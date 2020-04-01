using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class GetDirectoriesBehavior : IGetToscaServerDirectories
    {
        public IEnumerable<string> GetDirectories(string serverPath)
        {
            try
            {
                return Directory.GetDirectories(serverPath);
            }
            catch (Exception)
            {

                Trace.WriteLine("GetDirectories failed with an exception.");
                return new List<string>();
            }

        }
    }
}
