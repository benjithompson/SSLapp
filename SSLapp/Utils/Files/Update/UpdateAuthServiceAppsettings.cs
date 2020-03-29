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
        
        public UpdateAuthServiceAppsettings(){ }

        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            try
            {
                IEnumerable<string> appsettingsList = Directory.GetFiles(directoryPath, "appsettings.json");

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
                }
            }
            catch (Exception)
            {

                Trace.WriteLine("AuthService Update Exception.");
            }

        }
    }
}
