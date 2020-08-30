using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using SSLapp.Utils.Files.UpdateAgentHandlers;
using SSLapp.Utils.Files.Updates;

namespace SSLapp.Utils.Files.Update
{
    class UpdateDexAgentSettingsFactory : IUpdateSettingsFactory
    {
        public UpdateDexAgentSettingsFactory(){}

        public IUpdateFilesBehavior TryCreate(string appPath)
        {
            var appName = Path.GetFileName(appPath);
            switch (appName)
            {
                case "DistributedExecution":
                    return new UpdateDistributedExecution(appPath);
                default:
                    Trace.WriteLine("Application directory:\n " + appPath + "\ndoes not match an Updater.");
                    return null;
            }
        }
    }
}
