using System;
using System.Collections.Generic;
using System.Text;

namespace SSLapp.Utils.Files.Update
{
    interface IGetToscaServerDirectories
    {
        public IEnumerable<string> GetDirectories(string serverPath);
    }
}
