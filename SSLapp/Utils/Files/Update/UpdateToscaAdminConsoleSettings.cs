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
        public UpdateToscaAdminConsoleSettings(string appPath)
        {
            AppPath = appPath;
        }
        public string AppPath { get; set; }
        public bool Updated => throw new NotImplementedException();
        public int UpdatedFilesCount => throw new NotImplementedException();
        public void Update(ToscaConfigFilesModel config)
        {
            //update JSON
            try
            {
                Trace.WriteLine("Updating files in Administration Console");
                var appsetting = AppPath + @"\appsettings.json";
                string json = File.ReadAllText(appsetting);
                JObject jsonObj = JObject.Parse(json);
                Trace.WriteLine("---ServiceDiscovery.");
                UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                UpdateBaseUrl(jsonObj, config, appsetting);
                string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(appsetting, output);
            }
            catch (Exception)
            {

                throw;
            }


            //update web.config
            var webconfig = AppPath + @"\web.config";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(AppPath + @"\web.config");
                Trace.WriteLine("---CORS.");
                UpdateXMLFields.UpdateCORS(ref doc, config, webconfig);
            }
            catch (Exception)
            {
                Trace.WriteLine("Unable to update file at " + AppPath+@"\Web.config");
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
