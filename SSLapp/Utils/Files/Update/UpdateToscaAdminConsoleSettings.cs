using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSLapp.Models;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{
    class UpdateToscaAdminConsoleSettings : IUpdateFilesBehavior
    {

        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            //update JSON
            var appsetting = directoryPath + @"\appsettings.json";
            string json = File.ReadAllText(appsetting);
            JObject jsonObj = JObject.Parse(json);
            UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
            UpdateBaseUrl(jsonObj, config, appsetting);
            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(appsetting, output);

            //update web.config
            var webconfig = directoryPath + @"\web.config";
            XmlDocument doc = new XmlDocument();
            doc.Load(directoryPath + @"\web.config");
            UpdateXMLFields.UpdateCORS(ref doc, config, webconfig);
  

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
