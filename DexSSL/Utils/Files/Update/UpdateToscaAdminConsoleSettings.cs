using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{
    class UpdateToscaAdminConsoleSettings : IUpdateFilesBehavior
    {

        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            //update JSON
            IEnumerable<string> appsettingsList = Directory.GetFiles(directoryPath, "appsettings.json");
            foreach (var appsetting in appsettingsList)
            {
                string json = File.ReadAllText(appsetting);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                UpdateBaseUrl(jsonObj, config, appsetting);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appsetting, output);
            }

            //update web.config
            IEnumerable<string> webConfigList = Directory.GetFiles(directoryPath, "web.config");
            XmlDocument doc = new XmlDocument();
            foreach (var webconfig in webConfigList)
            {
                doc.Load(webconfig);
                UpdateXMLFields.UpdateCORS(ref doc, config, webconfig);
            }

        }

        public static void UpdateBaseUrl(dynamic jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                jsonObj["AdminConsoleSettings"]["BaseUrl"] = config.Hostname;
            }
            catch (Exception)
            {

                Console.WriteLine(appsetting + " does not have json field 'AdminConsoleSettings/BaseUrl'");
            }
            
        }
    }
}
