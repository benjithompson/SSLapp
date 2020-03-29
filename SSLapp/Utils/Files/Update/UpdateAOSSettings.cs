using SSLapp.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SSLapp.Utils.Files.Update
{
    class UpdateAOSSettings : IUpdateFilesBehavior
    {
        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            IEnumerable<string> appsettingsList = Directory.GetFiles(directoryPath, "appsettings.json");

            foreach (var appsetting in appsettingsList)
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
