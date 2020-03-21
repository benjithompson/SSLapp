using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSLapp.Models;

namespace SSLapp.Utils.Files
{
    class UpdateJSONFiles
    {
        public static void UpdateFile(string filepath, ToscaConfigFilesModel config)
        {

            string json = File.ReadAllText(filepath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            UpdateServiceDiscovery(jsonObj, config);
            UpdateScheme(jsonObj);

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            try
            {
                File.WriteAllText(filepath, output);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public static void UpdateField(string field, string value)
        {
            throw new NotImplementedException();
        }

        private static void UpdateServiceDiscovery(dynamic jsonObj, ToscaConfigFilesModel config)
        {
            var value = (string)jsonObj["Discovery"]["ServiceDiscovery"].Value;
            string[] sd = value.Split(':');
            var endpoint =  @"https://" + config.Hostname+":"+sd[2];
            jsonObj["Discovery"]["ServiceDiscovery"] = endpoint;

        }

        private static void UpdateBaseUrl(dynamic jsonObj, ToscaConfigFilesModel config)
        {

        }

        private static void UpdateScheme(dynamic jsonObj)
        {
            jsonObj["Discovery"]["Endpoints"][0]["Scheme"] = "https";
            jsonObj["Discovery"]["Endpoints"][1]["Scheme"] = "https";
            jsonObj["Discovery"]["Endpoints"][2]["Scheme"] = "https";
            jsonObj["HttpServer"]["Endpoints"]["Https"]["Scheme"] = "https";
        }

        private static void UpdateHost(dynamic jsonObj, ToscaConfigFilesModel config)
        {

        }
    }
}
