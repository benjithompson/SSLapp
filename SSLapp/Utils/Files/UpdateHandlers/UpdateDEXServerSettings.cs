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
            UpdatedFilesCount = 0;
            Updated = false;
        }
        public string AppPath { get; set; }
        public bool Updated { get; set; }
        public int UpdatedFilesCount { get; set; }
        public void Update(ToscaConfigFilesModel config)
        {
            XmlDocument doc;
            //update web.config
            try
            {
                var webconfig = AppPath + @"\web.config";
                doc = new XmlDocument();
                doc.Load(AppPath + @"\web.config");
                //doc.Load(@"C:\Program Files (x86)\TRICENTIS\Tosca Server\DEXServer\Web.config");
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
                Trace.WriteLine("Rdp Server endpoint node '/configuration/system.serviceModel/client/endpoint/address'not found in DEX Server web.config");
            }

            try
            {
                doc.SelectSingleNode("/configuration/system.serviceModel/bindings/basicHttpBinding/binding/security").Attributes["mode"].Value = "Transport";
                doc.SelectSingleNode("/configuration/system.serviceModel/bindings/webHttpBinding/binding/security").Attributes["mode"].Value = "Transport";
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                var baseAddresses = doc.SelectNodes("/configuration/system.serviceModel/services/service/host/baseAddresses/add");

                foreach (XmlNode baseAddress in baseAddresses)
                {
                    var split = baseAddress.Attributes["baseAddress"].Value.Split("/");
                    split[0] = "https:";
                    split[2] = (config.DexServerPort != null) ? config.Hostname : config.Hostname + ":" + config.DexServerPort;
                    var newBaseAddress = string.Empty;
                    foreach (var item in split)
                    {
                        newBaseAddress += item + "/";
                    }
                    baseAddress.Attributes["baseAddress"].Value = newBaseAddress;
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Rdp Server endpoint node '/configuration/system.serviceModel/client/endpoint/address'not found in DEX Server web.config");
            }

            using (FileStream fs = File.OpenWrite(AppPath + @"\Web.config"))
            {
                doc.Save(fs);
                UpdatedFilesCount++;
            }
            Updated = true;
        }
    }
}
