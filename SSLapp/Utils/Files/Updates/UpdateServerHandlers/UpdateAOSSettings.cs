using SSLapp.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SSLapp.Utils.Files.Update
{
    class UpdateAOSSettings : IUpdateFilesBehavior
    {
        public UpdateAOSSettings(string appPath)
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
                    Trace.WriteLine("Updating files in AO service");
                    string json = File.ReadAllText(appsetting);
                    JObject jsonObj = JObject.Parse(json);
                    Trace.WriteLine("---ServiceDiscovery.");
                    UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                    Trace.WriteLine("---Scheme.");
                    UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                    Trace.WriteLine("---HTTPS Thumbprint.");
                    UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                    Trace.WriteLine("---DexBaseUrl");
                    UpdateDexBaseUrl(jsonObj, config, appsetting);
                    string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
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

        public void UpdateDexBaseUrl(JObject jsonObj,ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                jsonObj["AutomationObjectServiceConfig"]["DexBaseUrl"] = @"https://" + config.Hostname + ":" + config.DexServerPort;
            }
            catch (Exception)
            {
                Trace.WriteLine(appsetting + " file doesn't contain 'AutomationObjectServiceConfig/DexBaseUrl'");
            }
        }
    }
}
