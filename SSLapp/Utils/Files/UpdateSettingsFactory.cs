using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace SSLapp.Utils.Files.Update
{
    class UpdateSettingsFactory
    {
        public UpdateSettingsFactory(){}

        public IUpdateFilesBehavior TryCreate(string appPath)
        {
            var appName = Path.GetFileName(appPath);
            switch (appName)
            {
                case "ServiceDiscovery":
                    return new UpdateServiceDiscoverySettings(appPath);
                case "AuthenticationService":
                    return new UpdateAuthServiceAppsettings(appPath);
                case "ProjectService":
                    return new UpdateProjectServiceSettings(appPath);
                case "MigrationService":
                    return new UpdateMigrationServiceSettings(appPath);
                case "ToscaAdministrationConsole":
                    return new UpdateToscaAdminConsoleSettings(appPath);
                case "AutomationObjectService":
                    return new UpdateAOSSettings(appPath);
                case "DexAdmin":
                    return new UpdateDexAdminsettings(appPath);
                case "DEXRdpServer":
                    return new UpdateDEXRdpServerSettings(appPath);
                case "DEXServer":
                    return new UpdateDEXServerSettings(appPath);
                case "FileService":
                    return new UpdateFileServiceSettings(appPath);
                case "LicenseAdministration":
                    return new UpdateLicenseAdministrationSettings(appPath);
                case "RESTApi":
                    return new UpdateRESTApiSettings(appPath);
                case "TestDataObjectViewer":
                    return new UpdateTestDataObjectViewerSettings(appPath);
                case "TestDataService":
                    return new UpdateTestDataServiceSettings(appPath);
                default:
                    Trace.WriteLine("Application directory:\n " + appPath + "\ndoes not match an Updater.");
                    return null;
            }
        }
    }
}
