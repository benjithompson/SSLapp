using System;
using SSLapp.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    static class UpdateJSONFields
    {
        public static void UpdateCertificate(JObject jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                jsonObj["HttpServer"]["Endpoints"]["Https"]["Thumbprint"] = config.GetCertificate.GetCertificateThumbprint();
                jsonObj["HttpServer"]["Endpoints"]["Https"]["StoreName"] = config.GetCertificate.GetCertificateStoreName();
                jsonObj["HttpServer"]["Endpoints"]["Https"]["StoreLocation"] = config.GetCertificate.GetCertificateStoreLocation();
            }
            catch (Exception)
            {
                Debug.WriteLine(appsetting + " file doesn't contain 'HttpServer/Endpoints/Https/(Thumbprint|StoreName|StoreLocation)'");
            }
        }

        public static void UpdateTokenCertificate(JObject jsonObj, ToscaConfigFilesModel config)
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

        public static void UpdateServiceDiscovery(JObject jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                var value = (string)jsonObj["Discovery"]["ServiceDiscovery"];
                string[] sd = value.Split(':');
                var endpoint = @"https://" + config.Hostname + ":" + sd[2];
                jsonObj["Discovery"]["ServiceDiscovery"] = endpoint;
            }
            catch (Exception)
            {
                Debug.WriteLine(appsetting + " file doesn't contain 'Discovery/ServiceDiscovery'");
            }

        }

        public static void UpdateScheme(JObject jsonObj, string appsetting)
        {
            try
            {
                var endpointsArray = jsonObj["Discovery"]["Endpoints"];
                foreach (var endpoint in endpointsArray)
                {
                    endpoint["Scheme"] = "https";
                }

            }
            catch (Exception)
            {
                Debug.WriteLine(appsetting+ " file doesn't contain Discovery/Endpoints");
            }
            try
            {
                jsonObj["HttpServer"]["Endpoints"]["Https"]["Scheme"] = "https";
            }
            catch (Exception)
            {

                Debug.WriteLine(appsetting + " doesn't contain 'HttpServer/Endpoints/Https/Scheme'");
            }
        }

        public static void UpdateHost(JObject jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                var endpointsArray = jsonObj["Discovery"]["Endpoints"];
                foreach (var endpoint in endpointsArray)
                {
                    endpoint["Host"] = config.Hostname;
                }
            }
            catch (Exception)
            {

                Debug.WriteLine(appsetting + " doesn't contain 'Discovery/Endpoints'");
            }

        }
    }
}
