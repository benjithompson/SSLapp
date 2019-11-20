using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;

namespace DexSSL
{
    public class ToscaConfigFiles : IDataErrorInfo
    {


        public string ServerConfig { get; set; }
        public string AgentConfig { get; set; }
        public string DexServerHostIP { get; set; }

        public string DexServerPort
        {
            get { return DexServerPort; }
            set { DexServerPort = DexServerHostIP.StartsWith("https") ? "443" : "80"; }
        }

        public string this[string name]{
            get { 
                string result = String.Empty;
                if (name == "ServerConfig")
                {
                    if (this.ServerConfig == "")
                    {
                        result = "Server web.config path must not be empty";
                    }
                }
                if (name == "AgentConfig")
                {
                    if (this.AgentConfig == "")
                    {
                        result = "Agent web.config path must not be empty";
                    }
                }
                if (name == "DexServerHostIP")
                {
                    if (this.DexServerHostIP == "")
                    {
                        result = "Host/IP must not be empty";
                    }
                }
                return result;
            }
        }


        public string Error => null;
    }
}
