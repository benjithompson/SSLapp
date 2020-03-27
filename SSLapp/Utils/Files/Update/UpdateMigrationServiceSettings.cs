using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSLapp.Models;

namespace SSLapp.Utils.Files.Update
{
    class UpdateMigrationServiceSettings : IUpdateFilesBehavior
    {
        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            IEnumerable<string> appsettingsList = Directory.GetFiles(directoryPath, "appsettings.json");

            foreach (var appsetting in appsettingsList)
            {
                Console.WriteLine("Updating JSON files in Migration service");
                string json = File.ReadAllText(appsetting);
                JObject jsonObj = JObject.Parse(json);
                Console.WriteLine("Updating fields:");
                Console.WriteLine("---ServiceDiscovery.");
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, directoryPath);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(appsetting, output);
            }
        }
    }
}
