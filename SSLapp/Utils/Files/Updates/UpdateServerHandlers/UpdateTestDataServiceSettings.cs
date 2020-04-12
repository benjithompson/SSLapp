using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Diagnostics;
using SSLapp.Utils.Files.Update;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{
    class UpdateTestDataServiceSettings : IUpdateFilesBehavior
    {
        public UpdateTestDataServiceSettings(string appPath)
        {
            AppPath = appPath;
            UpdatedFilesCount = 0;
            Updated = false;
        }
        public string AppPath { get; set; }
        public bool Updated { get; set; }
        public int UpdatedFilesCount { get; set; }
        public void Update(ToscaConfigFilesModel config)
        {
            try
            {
                IEnumerable<string> appsettingsList = Directory.GetFiles(AppPath, "appsettings.json");

                foreach (var appsetting in appsettingsList)
                {

                    Trace.WriteLine($"Updating files in {appsetting}");
                    string json = File.ReadAllText(appsetting);
                    JObject jsonObj = JObject.Parse(json);
                    Trace.WriteLine("---ServiceDiscovery.");
                    UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                    string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(appsetting, output);
                    UpdatedFilesCount++;
                }
            }catch (Exception)
            {

                Trace.WriteLine("Failed to updated file at " + AppPath);
            }

            //update web.config
            var webconfig = AppPath + @"\web.config";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(AppPath + @"\web.config");
                Trace.WriteLine("---CORS.");
                UpdateXMLFields.UpdateCORS(ref doc, config, webconfig);

                using (FileStream fs = File.Open(AppPath + @"\Web.config", FileMode.Create, FileAccess.Write))
                {
                    doc.Save(fs);
                    UpdatedFilesCount++;
                    Updated = true;
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Unable to update file at " + AppPath + @"\Web.config");
            }
        }
    }
}
