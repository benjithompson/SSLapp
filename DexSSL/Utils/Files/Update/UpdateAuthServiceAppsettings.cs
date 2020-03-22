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
                string json = File.ReadAllText(appsetting);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);

                UpdateServiceDiscovery(jsonObj, config);
                UpdateScheme(jsonObj);
                UpdateHost(jsonObj, config);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(appsetting, output);
            }
        }

        private void UpdateServiceDiscovery(dynamic jsonObj, ToscaConfigFilesModel config)
        {
            try
            {
                var value = (string)jsonObj["Discovery"]["ServiceDiscovery"].Value;
                string[] sd = value.Split(':');
                var endpoint = @"https://" + config.Hostname + ":" + sd[2];
                jsonObj["Discovery"]["ServiceDiscovery"] = endpoint;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void UpdateBaseUrl(dynamic jsonObj, ToscaConfigFilesModel config)
        {

        }

        private void UpdateScheme(dynamic jsonObj)
        {
            try
            {
                jsonObj["Discovery"]["Endpoints"][0]["Scheme"] = "https";
                jsonObj["Discovery"]["Endpoints"][1]["Scheme"] = "https";
                jsonObj["Discovery"]["Endpoints"][2]["Scheme"] = "https";
                jsonObj["HttpServer"]["Endpoints"]["Https"]["Scheme"] = "https";
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static void UpdateHost(dynamic jsonObj, ToscaConfigFilesModel config)
        {
            try
            {
                jsonObj["Discovery"]["Endpoints"][0]["Host"] = config.Hostname;
                jsonObj["Discovery"]["Endpoints"][1]["Host"] = config.Hostname;
                jsonObj["Discovery"]["Endpoints"][2]["Host"] = config.Hostname;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        private static void UpdateCertificate(dynamic jsonObj, ToscaConfigFilesModel config)
        {
            try
            {
                jsonObj["HttpServer"]["Endpoints"]["Https"]["Thumbprint"] = config.GetCertificate.GetCertificateThumbprint();
                jsonObj["HttpServer"]["Endpoints"]["Https"]["StoreName"] = config.GetCertificate.GetCertificateStoreName();
                jsonObj["HttpServer"]["Endpoints"]["Https"]["StoreLocation"] = config.GetCertificate.GetCertificateStoreLocation();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }


}
