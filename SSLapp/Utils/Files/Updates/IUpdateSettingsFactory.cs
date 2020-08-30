using SSLapp.Utils.Files.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSLapp.Utils.Files.Updates
{
    interface IUpdateSettingsFactory
    {
        public IUpdateFilesBehavior TryCreate(string appPath);
    }
}
