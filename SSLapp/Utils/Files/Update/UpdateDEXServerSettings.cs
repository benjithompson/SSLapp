using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using SSLapp.Models;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{
    class UpdateDEXServerSettings : IUpdateFilesBehavior
    {
        public void Update(string directoryPath, ToscaConfigFilesModel config)
        {
            //update web.config
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(directoryPath + @"\web.config");
                var rdpEndpoint = doc.SelectSingleNode("/configuration/system.serviceModel/client/endpoint").Attributes["address"].Value;
                var split = rdpEndpoint.Split(":");
                split[1] = @"://" + config.Hostname + ":";
                var newRdpEndpoint = string.Empty;
                foreach (var item in split)
                {
                    newRdpEndpoint += item;
                }
                doc.SelectSingleNode("/configuration/system.serviceModel/client/endpoint").Attributes["address"].Value = newRdpEndpoint;
                using (FileStream fs = File.OpenWrite(directoryPath + @"\web.config"))
                {
                    doc.Save(fs);
                }

            }
            catch (Exception)
            {

                Console.WriteLine("Web.config file not found in " + directoryPath);
            }

        }
    }
}
