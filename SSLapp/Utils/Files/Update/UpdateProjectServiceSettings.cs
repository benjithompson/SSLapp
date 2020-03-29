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
    class UpdateProjectServiceSettings : IUpdateFilesBehavior
    {
        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            IEnumerable<string> appsettingsList = Directory.GetFiles(directoryPath, "appsettings.json");

            foreach (var appsetting in appsettingsList)
            {
                Debug.WriteLine("Updating files in Project service");
                string json = File.ReadAllText(appsetting);
                JObject jsonObj = JObject.Parse(json);
                Debug.WriteLine("---ServiceDiscovery.");
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                Debug.WriteLine("---Scheme.");
                UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                Debug.WriteLine("---HTTPS Thumpbrint.");
                UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(appsetting, output);
            }
        }
    }
}
