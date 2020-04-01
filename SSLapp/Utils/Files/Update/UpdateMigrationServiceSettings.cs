using System;
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
        }
        public string AppPath { get; set; }
        public bool Updated => throw new NotImplementedException();
        public int UpdatedFilesCount => throw new NotImplementedException();
        public void Update(ToscaConfigFilesModel config)
        {
            IEnumerable<string> appsettingsList = Directory.GetFiles(AppPath, "appsettings.json");

            foreach (var appsetting in appsettingsList)
            {
                Trace.WriteLine("Updating files in Migration service");
                string json = File.ReadAllText(appsetting);
                JObject jsonObj = JObject.Parse(json);
                Trace.WriteLine("---ServiceDiscovery.");
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, AppPath);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(appsetting, output);
            }
        }
    }
}
