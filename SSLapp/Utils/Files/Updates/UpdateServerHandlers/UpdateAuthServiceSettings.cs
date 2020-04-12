using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SSLapp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{
    class UpdateAuthServiceSettings : IUpdateFilesBehavior
    {
        public UpdateAuthServiceSettings(string appPath)
        {
            AppPath = appPath;
            UpdatedFilesCount = 0;
            Updated = false;
        }
        public string AppPath { get ; set; }
        public bool Updated { get; set; }
        public int UpdatedFilesCount { get; set; }
        public void Update(ToscaConfigFilesModel config)
        {
            try
            {
                IEnumerable<string> appsettingsList = Directory.GetFiles(AppPath, "appsettings.json");

                foreach (var appsetting in appsettingsList)
                {

                    Trace.WriteLine("Updating files in Authentication service");
                    string json = File.ReadAllText(appsetting);
                    JObject jsonObj = JObject.Parse(json);
                    Trace.WriteLine("---ServiceDiscovery.");
                    UpdateJSONFields.UpdateServiceDiscovery(jsonObj, config, appsetting);
                    Trace.WriteLine("---Scheme.");
                    UpdateJSONFields.UpdateScheme(jsonObj, appsetting);
                    Trace.WriteLine("---Host.");
                    UpdateJSONFields.UpdateHost(jsonObj, config, appsetting);
                    Trace.WriteLine("---HTTPS Thumbprint.");
                    UpdateJSONFields.UpdateCertificate(jsonObj, config, appsetting);
                    Trace.WriteLine("---Token Thumbprint.");
                    UpdateJSONFields.UpdateTokenCertificate(jsonObj, config);
                    UpdateDataXml(AppPath, config);
                    string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(appsetting, output);
                    UpdatedFilesCount++;

                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Failed to updated file at " + AppPath);
            }


        }

        private void UpdateDataXml(string appPath, ToscaConfigFilesModel config)
        {
            XmlDocument doc;
            var dataxml = string.Empty;
            //update web.config
            try
            {

                appPath = appPath.Replace("AuthenticationService", "LandingPage");
                dataxml = appPath + @"\resources\data\data.xml";
                doc = new XmlDocument();
                doc.Load(dataxml);
                //doc.Load(@"C:\Program Files (x86)\TRICENTIS\Tosca Server\DEXServer\Web.config");
            }
            catch (Exception)
            {

                Trace.WriteLine("Web.config file not found at" + AppPath + @"\Web.config");
                return;
            }

            try
            {
                var hostname = doc.SelectSingleNode("/features/feature/url");
                var split = hostname.InnerText.Split(":");
                hostname.InnerText = "https://" + config.Hostname + ":" + split[2];
            }
            catch (Exception)
            {
                Trace.WriteLine("Rdp Server endpoint node '/configuration/system.serviceModel/client/endpoint/address'not found in DEX Server web.config");
                return;
            }
            using (FileStream fs = File.Open(dataxml, FileMode.Create, FileAccess.Write))
            {
                doc.Save(fs);
                UpdatedFilesCount++;
                Updated = true;
            }
        }
    }
}
