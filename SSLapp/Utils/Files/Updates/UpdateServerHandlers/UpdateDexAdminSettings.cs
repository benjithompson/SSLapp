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
    class UpdateDexAdminsettings : IUpdateFilesBehavior
    {
        public UpdateDexAdminsettings(string appPath)
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
            //update web.config
            var webconfig = AppPath + @"\web.config";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(AppPath + @"\web.config");
                Trace.WriteLine("---CORS.");
                UpdateXMLFields.UpdateCORS(ref doc, config, webconfig);

                using (FileStream fs = File.Open(AppPath + @"\Web.config", FileMode.Create, FileAccess.Write))
                {
                    doc.Save(fs);
                    UpdatedFilesCount++;
                    Updated = true;
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Unable to update file at " + AppPath + @"\Web.config");
            }
        }
    }
}
