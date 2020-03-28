using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using System.IO;

namespace SSLapp.Utils.Files.Update
{
    class BaseFileUpdateHandler
    {
        List<Tuple<string, IUpdateFilesBehavior>> _updateFilesBehaviorList = new List<Tuple<string, IUpdateFilesBehavior>>();
        IGetToscaServerDirectories _getDirectoriesBehavior;
        ToscaConfigFilesModel _config;

        public BaseFileUpdateHandler(ToscaConfigFilesModel config)
        {
            _config = config;
        }

        public BaseFileUpdateHandler(IGetToscaServerDirectories getToscaServerDirectories, ToscaConfigFilesModel config)
        {
            _getDirectoriesBehavior = getToscaServerDirectories;
            _config = config;
        }

        public void AddFileUpdateBehavior(string toscaServerAppName)
        {
            var appname = Path.GetFileName(toscaServerAppName);
            switch (appname)
            {
                case "ServiceDiscovery":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateServiceDiscoverySettings()));
                    break;
                case "AuthenticationService":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateAuthServiceAppsettings()));
                    break;
                case "ProjectService":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateProjectServiceSettings()));
                    break;
                case "MigrationService":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateMigrationServiceSettings()));
                    break;
                case "ToscaAdministrationConsole":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateToscaAdminConsoleSettings()));
                    break;
                case "AutomationObjectService":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateServiceDiscoverySettings()));
                    break;
                case "DexAdmin":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateDexAdminAppsettings()));
                    break;
                case "DEXRdpServer":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateDEXRdpServerSettings()));
                    break;
                case "DEXServer":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateDEXServerSettings()));
                    break;
                case "FileService":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateFileServiceSettings()));
                    break;
                case "LicenseAdministration":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateLicenseAdministrationSettings()));
                    break;
                case "RESTApi":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateRESTApiSettings()));
                    break;
                case "TestDataObjectViewer":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateTestDataObjectViewerSettings()));
                    break;
                case "TestDataService":
                    _updateFilesBehaviorList.Add(new Tuple<string, IUpdateFilesBehavior>(toscaServerAppName, new UpdateTestDataServiceSettings()));
                    break;
                default:
                    //TODO: Log unmatched toscaServerAppName (folder) 
                    break;
            }
        }

        public void UpdateAll()
        {
            foreach (var updater in _updateFilesBehaviorList)
            {
                var path = updater.Item1;
                var behavior = updater.Item2;
                behavior.Update(path, _config);
            }
        }

        public IEnumerable<string> GetToscaServerDirectories(string serverPath)
        {
            return _getDirectoriesBehavior.GetDirectories(serverPath);
        }
    }
}
