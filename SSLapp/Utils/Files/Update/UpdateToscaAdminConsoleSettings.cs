using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
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
            Trace.WriteLine("Updating files in Administration Console");
            var appsetting = directoryPath + @"\appsettings.json";
            string json = File.ReadAllText(appsetting);
            JObject jsonObj = JObject.Parse(json);
            Trace.WriteLine("---ServiceDiscovery.");
            UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
            UpdateBaseUrl(jsonObj, config, appsetting);
            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(appsetting, output);

            //update web.config
            var webconfig = directoryPath + @"\web.config";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(directoryPath + @"\web.config");
                Trace.WriteLine("---CORS.");
                UpdateXMLFields.UpdateCORS(ref doc, config, webconfig);
            }
            catch (Exception)
            {
                Trace.WriteLine("Unable to update file at " + directoryPath+@"\Web.config");
            }

        }

        public static void UpdateBaseUrl(JObject jsonObj, ToscaConfigFilesModel config, string appsetting)
        {
            try
            {
                jsonObj["AdminConsoleSettings"]["BaseUrl"] = config.Hostname;
            }
            catch (Exception)
            {

                Trace.WriteLine(appsetting + " does not have json field 'AdminConsoleSettings/BaseUrl'");
            }
            
        }
    }
}
