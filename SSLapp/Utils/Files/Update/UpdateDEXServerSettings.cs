using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Xml;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateDEXServerSettings : IUpdateFilesBehavior
    {
        public UpdateDEXServerSettings(string appPath)
        {
            AppPath = appPath;
        }
        public string AppPath { get; set; }
        public bool Updated => throw new NotImplementedException();
        public int UpdatedFilesCount => throw new NotImplementedException();
        public void Update(ToscaConfigFilesModel config)
        {
            XmlDocument doc;
            //update web.config
            try
            {
                var webconfig = AppPath + @"\web.config";
                doc = new XmlDocument();
                //doc.Load(AppPath + @"\web.config");
                doc.Load(@"C:\Program Files (x86)\TRICENTIS\Tosca Server\DEXServer\Web.config");
            }
            catch (Exception)
            {

                Trace.WriteLine("Web.config file not found at" + AppPath + @"\Web.config");
                return;
            }

            try
            {
                var rdpEndpoint = doc.SelectSingleNode("/configuration/system.serviceModel/client/endpoint").Attributes["address"];
                var split = rdpEndpoint.Value.Split(":");
                split[1] = @"://" + config.Hostname + ":";
                var newRdpEndpoint = string.Empty;
                foreach (var item in split)
                {
                    newRdpEndpoint += item;
                }
                rdpEndpoint.Value = newRdpEndpoint;
            }
            catch (Exception)
            {
                Trace.WriteLine("Node '/configuration/system.serviceModel/client/endpoint/address'not found in DEX Server web.config");
            }

            using (FileStream fs = File.OpenWrite(AppPath + @"\Web.config"))
            {
                doc.Save(fs);
            }
        }
    }
}
