using SSLapp.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace SSLapp.Utils.Files.Update
{
    /*
 * 1. Open your ToscaRDPServer.exe.config file and search for:
        <endpoint address="" binding="basicHttpBinding" bindingConfiguration="" contract="Tricentis.RdpServer.IRdpSlaveService"/> 
        Insert "RdpSlaveServiceBinding"
   2. Replace all "http://localhost" with "https://<Host name>:<Server Port>"
   3. Search for  <serviceMetadata httpGetEnabled="true"/> 
        --> Change to: <serviceMetadata httpGetEnabled="true" httpsGetEnabled="true" />
   4. Set security mode Transport
   5. Bind the certificate: netsh http add sslcert ipport=0.0.0.0:9000 certhash=<Certificate Thumbprint without whitespaces> appid={00112233-4455-6677-8899-AABBCCDDEEFF}
 */
    class UpdateDEXRdpServerSettings : IUpdateFilesBehavior
    {
        public UpdateDEXRdpServerSettings(string appPath)
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
            Trace.WriteLine("RdpServer Update:");

            XmlDocument doc;
            var exeConfig = string.Empty;
            try
            {
                exeConfig = AppPath + @"\ToscaRdpServer.exe.config";
                doc = new XmlDocument();
                doc.Load(exeConfig);
            }
            catch (Exception)
            {

                Trace.WriteLine("exe.config file not found at" + exeConfig);
                return;
            }

            try
            {
                //1.
                var binding = doc.SelectSingleNode("/configuration/system.serviceModel/services/service/endpoint").Attributes["bindingConfiguration"];
                binding.Value = "RdpSlaveServiceBinding";

                //2.
                var rdpEndpoint = doc.SelectSingleNode("/configuration/system.serviceModel/services/service/host/baseAddresses/add").Attributes["baseAddress"];
                var split = rdpEndpoint.Value.Split(":");
                var newRdpEndpoint = split[1] = @"https://" + config.Hostname + ":" + split[2];
                rdpEndpoint.Value = newRdpEndpoint;

                var baseAddress = doc.SelectSingleNode("/configuration/system.serviceModel/client/endpoint").Attributes["address"];
                split = baseAddress.Value.Split("/");
                split[0] = "https:";
                split[2] = (string.IsNullOrEmpty(config.DexServerPort)) ? config.Hostname : config.Hostname + ":" + config.DexServerPort;
                var newBaseAddress = string.Empty;
                for (var i = 0; i < split.Length; i++)
                {
                    if (i < split.Length - 1)
                    {
                        newBaseAddress += split[i] + "/";

                    }
                    else
                    {
                        newBaseAddress += split[i];
                    }
                }
                baseAddress.Value = newBaseAddress;

                //3.
                if (doc.SelectSingleNode("/configuration/system.serviceModel/behaviors/serviceBehaviors/behavior/serviceMetadata").Attributes["httpsGetEnabled"] == null)
                {
                    XmlAttribute httpsAttr = doc.CreateAttribute("httpsGetEnabled");
                    httpsAttr.Value = "true";
                    XmlNode httpsGetEnableNode = doc.SelectSingleNode("/configuration/system.serviceModel/behaviors/serviceBehaviors/behavior/serviceMetadata");
                    httpsGetEnableNode.Attributes.Append(httpsAttr);
                }

                //4.
                doc.SelectSingleNode("/configuration/system.serviceModel/bindings/basicHttpBinding/binding/security").Attributes["mode"].Value = "Transport";

                if(doc.SelectSingleNode("//binding[@name='BasicHttpBinding_IRdpMasterService']/security") == null)
                {
                    XmlElement security = doc.CreateElement("security");
                    security.SetAttribute("mode", "Transport");
                    var node = doc.SelectSingleNode("//binding[@name='BasicHttpBinding_IRdpMasterService']");
                    node.AppendChild(security);
                }

                //5. Bind cert to port
                BackgroundWorker worker_SslCert = new BackgroundWorker();
                worker_SslCert.WorkerReportsProgress = false;
                worker_SslCert.DoWork += DoAsyncSslCertBinding;
                worker_SslCert.RunWorkerAsync(argument: config.CertThumbprint);

            }
            catch (Exception)
            {
                Trace.WriteLine("Exception binding rdp certificate");
                return;
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
                    Trace.WriteLine("Exception saving ToscaRdpServer.exe.config");
                    return;
                }

            }


        }
        public static void DoAsyncSslCertBinding(object o, DoWorkEventArgs args)
        {
            Trace.WriteLine("Bind the certificate: netsh http add sslcert...");
            using (Process bindProc = new Process())
            {

                bindProc.StartInfo.FileName = "netsh.exe";
                bindProc.StartInfo.RedirectStandardOutput = true;
                bindProc.StartInfo.RedirectStandardError = true;
                bindProc.StartInfo.CreateNoWindow = true;

                //delete
                Trace.WriteLine("http delete sslcert ipport=0.0.0.0:9000");
                bindProc.StartInfo.Arguments = "http delete sslcert ipport=0.0.0.0:9000";
                bindProc.Start();
                bindProc.WaitForExit();

                //Add
                Trace.WriteLine("http add sslcert ipport=0.0.0.0:9000 certhash=" + args.Argument + " appid={00112233-4455-6677-8899-AABBCCDDEEFF}");
                bindProc.StartInfo.Arguments = "http add sslcert ipport=0.0.0.0:9000 certhash=" + args.Argument + " appid ={00112233-4455-6677-8899-AABBCCDDEEFF}";
                bindProc.Start();
                bindProc.WaitForExit();
                string output = bindProc.StandardOutput.ReadToEnd();
                Trace.WriteLine(output);
            }
        }
    }
}
