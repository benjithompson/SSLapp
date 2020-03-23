using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SSLapp.Utils.Files.Update
{
    class GetDirectoriesBehavior : IGetToscaServerDirectories
    {
        public IEnumerable<string> GetDirectories(string serverPath)
        {
            return Directory.GetDirectories(serverPath);
        }
    }
}
