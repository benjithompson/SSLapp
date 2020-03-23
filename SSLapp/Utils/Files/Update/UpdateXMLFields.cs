using System;
using System.Collections.Generic;
using System.Text;
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
                var corsValue = xmlObj.SelectSingleNode("/configuration/system.webServer/httpProtocol/customHeaders/add").Attributes["value"].Value;
            }
            catch (Exception)
            {

                Console.WriteLine(webconfig + " XML error while adding value");
            }
            
        }
    }
}
