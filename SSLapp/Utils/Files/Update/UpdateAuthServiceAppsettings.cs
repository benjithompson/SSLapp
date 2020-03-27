﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SSLapp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SSLapp.Utils.Files.Update
{
    class UpdateAuthServiceAppsettings : IUpdateFilesBehavior
    {
        
        public UpdateAuthServiceAppsettings(){ }

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
                Console.WriteLine("---Host.");
                UpdateJSONFields.UpdateHost(jsonObj, config, appsetting);
                Console.WriteLine("---HTTPS Thumbprint.");
                UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                Console.WriteLine("---Token Thumbprint.");
                UpdateJSONFields.UpdateTokenCertificate(jsonObj, config);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                Console.WriteLine("writing updated file...");
                File.WriteAllText(appsetting, output);
                Console.WriteLine("Update complete!");
            }
        }
    }
}
