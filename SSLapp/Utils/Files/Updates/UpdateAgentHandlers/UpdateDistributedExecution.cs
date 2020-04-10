using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using System.Xml;
using System.Diagnostics;
using System.IO;
using SSLapp.Utils.Files.Update;

namespace SSLapp.Utils.Files.UpdateAgentHandlers
{
    class UpdateDistributedExecution : IUpdateFilesBehavior
    {
        public UpdateDistributedExecution(string appPath)
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
            var exeConfig = AppPath + @"\ToscaDistributionAgent.exe.config";
            try
            {
                
                doc = new XmlDocument();
                doc.Load(exeConfig);
            }
            catch (Exception)
            {

                Trace.WriteLine("exe.config file not found at " + exeConfig);
                return;
            }

            try
            {
                if(config.Hostname != null)
                {
                    var address = doc.SelectSingleNode("/configuration/system.serviceModel/client/endpoint").Attributes["address"];
                    var split = address.Value.Split("/");
                    split[0] = "https:";
                    split[2] = (string.IsNullOrEmpty(config.DexServerPort)) ? config.Hostname : config.Hostname + ":" + config.DexServerPort;
                    var newAddress = string.Empty;
                    for (int i = 0; i < split.Length; i++)
                    {
                        if(i < split.Length-1)
                        {
                            newAddress += split[i] + "/";

                        }
                        else
                        {
                            newAddress += split[i];
                        }

                    }
                    address.Value = newAddress;
                }

            }
            catch (Exception)
            {
                Trace.WriteLine("Agent address node '/configuration/system.serviceModel/client/endpoint/address'not found in Agent exe.config");
            }

            try
            {
                doc.SelectSingleNode("/configuration/system.serviceModel/bindings/basicHttpBinding/binding/security").Attributes["mode"].Value = "Transport";
            }
            catch (Exception)
            {
                Trace.WriteLine("Agent address node '/configuration/system.serviceModel/bindings/basicHttpBinding/binding/security' not found in Agent exe.config");
            }

            using (FileStream fs = File.OpenWrite(exeConfig))
            {
                doc.Save(fs);
                UpdatedFilesCount++;
            }
            Updated = true;
        }
    }
}
