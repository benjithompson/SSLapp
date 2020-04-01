using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateFileServiceSettings : IUpdateFilesBehavior
    {
        public UpdateFileServiceSettings(string appPath)
        {
            AppPath = appPath;
        }
        public string AppPath { get; set; }
        public bool Updated => throw new NotImplementedException();
        public int UpdatedFilesCount => throw new NotImplementedException();
        public void Update(ToscaConfigFilesModel config)
        {
            Trace.WriteLine("File Service not implemented");
        }
    }
}
