using System;
using System.Collections.Generic;
using System.Text;

namespace SSLapp.Utils.Files.Update
{
    interface IGetInstallationDirectories
    {
        public IEnumerable<string> GetAppPathList(string appPath);
    }
}
