using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using Newtonsoft.Json.Linq;

namespace SSLapp.Utils.Files.Update
{
    static class UpdateJSONFields
    {
        public static void UpdateCertificate(dynamic jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                jsonObj["HttpServer"]["Endpoints"]["Https"]["Thumbprint"] = config.GetCertificate.GetCertificateThumbprint();
                jsonObj["HttpServer"]["Endpoints"]["Https"]["StoreName"] = config.GetCertificate.GetCertificateStoreName();
                jsonObj["HttpServer"]["Endpoints"]["Https"]["StoreLocation"] = config.GetCertificate.GetCertificateStoreLocation();
            }
            catch (Exception)
            {
                Console.WriteLine(appsetting + " file doesn't contain 'HttpServer/Endpoints/Https/(Thumbprint|StoreName|StoreLocation)'");
            }
        }

        public static void UpdateServiceDiscovery(dynamic jsonObj, ToscaConfigFilesModel config, string appsetting)
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
                Console.WriteLine(appsetting + " file doesn't contain 'Discovery/ServiceDiscovery'");
            }

        }

        public static void UpdateScheme(dynamic jsonObj, string appsetting)
        {
            try
            {
                var endpointsArray = (JArray)jsonObj["Discovery"]["Endpoints"];
                foreach (var endpoint in endpointsArray)
                {
                    endpoint["Scheme"] = "https";
                }

            }
            catch (Exception)
            {
                Console.WriteLine(appsetting+ " file doesn't contain Discovery/Endpoints");
            }
            try
            {
                jsonObj["HttpServer"]["Endpoints"]["Https"]["Scheme"] = "https";
            }
            catch (Exception)
            {

                Console.WriteLine(appsetting + " doesn't contain 'HttpServer/Endpoints/Https/Scheme'");
            }
        }

        public static void UpdateHost(dynamic jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                var endpointsArray = (JArray)jsonObj["Discovery"]["Endpoints"];
                foreach (var endpoint in endpointsArray)
                {
                    endpoint["Host"] = config.Hostname;
                }
            }
            catch (Exception)
            {

                Console.WriteLine(appsetting + " doesn't contain 'Discovery/Endpoints'");
            }

        }
    }
}
