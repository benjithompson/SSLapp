using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SSLapp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateAuthServiceAppsettings : IUpdateFilesBehavior
    {
        public UpdateAuthServiceAppsettings(string appPath)
        {
            AppPath = appPath;
            UpdatedFilesCount = 0;
            Updated = false;
        }
        public string AppPath { get ; set; }
        public bool Updated { get; set; }
        public int UpdatedFilesCount { get; set; }
        public void Update(ToscaConfigFilesModel config)
        {
            try
            {
                IEnumerable<string> appsettingsList = Directory.GetFiles(AppPath, "appsettings.json");

                foreach (var appsetting in appsettingsList)
                {

                    Trace.WriteLine("Updating files in Authentication service");
                    string json = File.ReadAllText(appsetting);
                    JObject jsonObj = JObject.Parse(json);
                    Trace.WriteLine("---ServiceDiscovery.");
                    UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                    Trace.WriteLine("---Scheme.");
                    UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                    Trace.WriteLine("---Host.");
                    UpdateJSONFields.UpdateHost(jsonObj, config, appsetting);
                    Trace.WriteLine("---HTTPS Thumbprint.");
                    UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                    Trace.WriteLine("---Token Thumbprint.");
                    UpdateJSONFields.UpdateTokenCertificate(jsonObj, config);
                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(appsetting, output);
                    UpdatedFilesCount++;
                }
            }
            catch (Exception)
            {

                Trace.WriteLine("Failed to updated file at " + AppPath);
            }
            Updated = true;

        }
    }
}
