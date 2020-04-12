using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Diagnostics;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{

    /*
* 1. Open your ToscaRDPServer.exe.config file and search for:
    <endpoint address="" binding="basicHttpBinding" bindingConfiguration="" contract="Tricentis.RdpServer.IRdpSlaveService"/> 
    Change to "https"
*/

    class UpdateRESTApiSettings : IUpdateFilesBehavior
    {
        public UpdateRESTApiSettings(string appPath)
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
            Trace.WriteLine("REST API Update:");

            XmlDocument doc;
            var exeConfig = string.Empty;
            try
            {
                exeConfig = AppPath + @"\web.config";
                doc = new XmlDocument();
                doc.Load(exeConfig);
            }
            catch (Exception)
            {

                Trace.WriteLine("web.config file not found at" + exeConfig);
                return;
            }

            try
            {
                //1.
                var binding = doc.SelectSingleNode("/configuration/system.serviceModel/services/service/endpoint").Attributes["bindingConfiguration"];
                binding.Value = "https";
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception changing binding to 'https'");
                throw;
            }

            using (FileStream fs = File.Open(exeConfig, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    doc.Save(fs);
                    UpdatedFilesCount++;
                    Updated = true;
                }
                catch (Exception)
                {
                    Trace.WriteLine("Exception saving REST API web.config");
                    throw;
                }
            }
        }
    }
}
