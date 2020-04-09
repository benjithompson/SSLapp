using System;
using System.Collections.Generic;
using System.Text;

namespace SSLapp.Utils.Files.Update
{
    interface IGetToscaApps
    {
        public IEnumerable<string> GetToscaApps(string appPath);
    }
}
