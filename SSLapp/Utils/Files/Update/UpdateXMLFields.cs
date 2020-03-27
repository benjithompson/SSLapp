using System;
using System.IO;
using System.Xml;
using SSLapp.Models;

namespace SSLapp.Utils.Files.Update
{
    class UpdateXMLFields
    {
        public static void UpdateCORS(ref XmlDocument xmlObj, ToscaConfigFilesModel config, string webconfig)
        {
            try
            {
                var corsValue = xmlObj.SelectSingleNode("/configuration/system.webServer/httpProtocol/customHeaders/add").Attributes["value"];
                var newCorsValue = @"default-src " + config.Hostname + @":* 'self' 'unsafe-inline';frame-src " + config.Hostname + @":* 'self' localhost:*; connect-src " +
                    config.Hostname + @":* 'self' localhost:*; script-src 'self' 'unsafe-inline' https://ajax.googleapis.com https://maxcdn.bootstrapcdn.com";
                corsValue.Value = newCorsValue;
                xmlObj.Save(webconfig);
            }
            catch (Exception)
            {

                Console.WriteLine(webconfig + " XML error while adding value");
            }
            
        }
    }
}
