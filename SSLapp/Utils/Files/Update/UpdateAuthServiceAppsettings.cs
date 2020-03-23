using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;

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
                Console.WriteLine("file opened. Serializing to JSON");
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                Console.WriteLine("jsonObj created");
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                UpdateJSONFields.UpdateHost(jsonObj, config, appsetting);
                UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                UpdateTokenCertificate(jsonObj, config);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(appsetting, output);
            }
        }
        public static void UpdateTokenCertificate(dynamic jsonObj, ToscaConfigFilesModel config)
        {
            try
            {
                jsonObj["TokenSignCertificate"]["Thumbprint"] = config.GetCertificate.GetCertificateThumbprint();
                jsonObj["TokenSignCertificate"]["StoreName"] = config.GetCertificate.GetCertificateStoreName();
                jsonObj["TokenSignCertificate"]["StoreLocation"] = config.GetCertificate.GetCertificateStoreLocation();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
