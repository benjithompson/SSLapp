using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using System.IO;

namespace SSLapp.Utils.Files.Update
{
    class BaseFileUpdateHandler
    {
        IUpdateFilesBehavior _updateFilesBehavior;
        IGetToscaServerDirectories _getDirectoriesBehavior;
        ToscaConfigFilesModel _config;

        public BaseFileUpdateHandler(ToscaConfigFilesModel config)
        {
            _config = config;
        }

        public BaseFileUpdateHandler(IUpdateFilesBehavior updateFileBehavior, IGetToscaServerDirectories getToscaServerDirectories, ToscaConfigFilesModel config)
        {
            _updateFilesBehavior = updateFileBehavior;
            _getDirectoriesBehavior = getToscaServerDirectories;
            _config = config;
        }

        public void SetFileUpdateBehavior(IUpdateFilesBehavior updateBehavior)
        {
            _updateFilesBehavior = updateBehavior;
        }
        public void SetFileUpdateBehavior(string toscaServerAppName)
        {
            switch (toscaServerAppName)
            {
                case "AuthenticationService":
                    _updateFilesBehavior = new UpdateAuthServiceAppsettings();
                    break;
                case "AutomationObjectService":
                    _updateFilesBehavior = new UpdateAOSSettings();
                    break;
                case "DexAdmin":
                    _updateFilesBehavior = new UpdateDexAdminAppsettings();
                    break;
                case "DEXRdpServer":
                    _updateFilesBehavior = new UpdateDEXRdpServerSettings();
                    break;
                case "DEXServer":
                    _updateFilesBehavior = new UpdateDEXServerSettings();
                    break;
                case "FileService":
                    _updateFilesBehavior = new UpdateFileServiceSettings();
                    break;
                case "LicenseAdministration":
                    _updateFilesBehavior = new UpdateLicenseAdministrationSettings();
                    break;
                case "MigrationService":
                    _updateFilesBehavior = new UpdateMigrationServiceSettings();
                    break;
                case "ProjectService":
                    _updateFilesBehavior = new UpdateProjectServiceSettings();
                    break;
                case "RESTApi":
                    _updateFilesBehavior = new UpdateRESTApiSettings();
                    break;
                case "ServiceDiscovery":
                    _updateFilesBehavior = new UpdateServiceDiscoverySettings();
                    break;
                case "TestDataObjectViewer":
                    _updateFilesBehavior = new UpdateTestDataObjectViewerSettings();
                    break;
                case "TestDataService":
                    _updateFilesBehavior = new UpdateTestDataServiceSettings();
                    break;
                case "ToscaAdministrationConsole":
                    _updateFilesBehavior = new UpdateToscaAdminConsoleSettings();
                    break;
                default:
                    break;
            }
        }


        public void Update(string directoryPath)
        {
            SetFileUpdateBehavior(Path.GetFileName(directoryPath));
            _updateFilesBehavior.Update(directoryPath, _config);
        }

        public IEnumerable<string> GetToscaServerDirectories(string serverPath)
        {
            return _getDirectoriesBehavior.GetDirectories(serverPath);
        }
    }
}
