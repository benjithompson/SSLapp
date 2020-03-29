using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateLicenseAdministrationSettings : IUpdateFilesBehavior
    {
        public void Update(string filepath, ToscaConfigFilesModel config)
        {
            Debug.WriteLine("License Administration not implemented");
        }
    }
}
