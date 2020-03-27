using SSLapp.Models;
using System;
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

                Console.WriteLine("Updating JSON files in Authentication service");
                string json = File.ReadAllText(appsetting);
                Console.WriteLine("Appsettings.json opened. Serializing to JSON...");
                JObject jsonObj = JObject.Parse(json);
                Console.WriteLine("Serialized!");
                Console.WriteLine("Updating fields:");
                Console.WriteLine("---ServiceDiscovery.");
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                Console.WriteLine("---Scheme.");
                UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                Console.WriteLine("---HTTPS Thumbprint.");
                UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                Console.WriteLine("---DexBaseUrl");
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
                Console.WriteLine(appsetting + " file doesn't contain 'AutomationObjectServiceConfig/DexBaseUrl'");
            }
        }
    }
}
