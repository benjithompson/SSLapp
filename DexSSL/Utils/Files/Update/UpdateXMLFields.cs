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
                xmlObj.SelectSingleNode("/appSettings/add").Attributes["value"].Value = "hello";
            }
            catch (Exception)
            {

                Console.WriteLine(webconfig + " XML error while adding value");
            }
            
        }
    }
}
