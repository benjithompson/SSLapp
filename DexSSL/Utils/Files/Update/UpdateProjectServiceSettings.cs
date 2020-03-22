using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;

namespace SSLapp.Utils.Files.Update
{
    class UpdateProjectServiceSettings : IUpdateFilesBehavior
    {
        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            IEnumerable<string> appsettingsList = Directory.GetFiles(directoryPath, "appsettings.json");

            foreach (var appsetting in appsettingsList)
            {
                string json = File.ReadAllText(appsetting);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);

                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(appsetting, output);
            }
        }
    }
}
