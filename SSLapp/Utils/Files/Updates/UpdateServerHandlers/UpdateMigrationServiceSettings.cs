﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSLapp.Models;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateMigrationServiceSettings : IUpdateFilesBehavior
    {
        public UpdateMigrationServiceSettings(string appPath)
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
            IEnumerable<string> appsettingsList = Directory.GetFiles(AppPath, "appsettings.json");

            foreach (var appsetting in appsettingsList)
            {
                try
                {
                    Trace.WriteLine("Updating files in Migration service");
                    string json = File.ReadAllText(appsetting);
                    JObject jsonObj = JObject.Parse(json);
                    Trace.WriteLine("---ServiceDiscovery.");
                    UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, AppPath);
                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(appsetting, output);
                    UpdatedFilesCount++;
                    Updated = true;
                }
                catch (Exception)
                {
                    Trace.WriteLine("Failed to updated file at " + AppPath);
                }

            }

        }
    }
}
