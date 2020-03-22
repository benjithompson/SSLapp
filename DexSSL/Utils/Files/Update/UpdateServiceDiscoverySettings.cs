using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;

namespace SSLapp.Utils.Files.Update
{
    class UpdateServiceDiscoverySettings : IUpdateFilesBehavior
    {
        public void Update(string filepath, ToscaConfigFilesModel config)
        {

        }

        public void Update(IEnumerable<string> filelist, ToscaConfigFilesModel config)
        {
            throw new NotImplementedException();
        }

        private void UpdateServiceDiscovery(dynamic jsonObj, ToscaConfigFilesModel config)
        {
            var value = (string)jsonObj["Discovery"]["ServiceDiscovery"].Value;
            string[] sd = value.Split(':');
            var endpoint = @"https://" + config.Hostname + ":" + sd[2];
            jsonObj["Discovery"]["ServiceDiscovery"] = endpoint;
        }

        private void UpdateBaseUrl(dynamic jsonObj, ToscaConfigFilesModel config)
        {

        }

        private void UpdateScheme(dynamic jsonObj)
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
