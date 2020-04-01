﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateTestDataServiceSettings : IUpdateFilesBehavior
    {
        public UpdateTestDataServiceSettings(string appPath)
        {
            AppPath = appPath;
            UpdatedFilesCount = 0;
            Updated = false;
        }
        public string AppPath { get; set; }
        public bool Updated { get; set; }
        public int UpdatedFilesCount { get; set; }
        public void Update(ToscaConfigFilesModel config)
        {
            Trace.WriteLine("TDS not implemented");
        }
    }
}
