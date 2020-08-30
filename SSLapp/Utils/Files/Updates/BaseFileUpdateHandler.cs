using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using System.IO;
using System.Diagnostics;
using System.Linq;
using SSLapp.ViewModels;
using SSLapp.Utils.Files.Updates;

namespace SSLapp.Utils.Files.Update
{
    class BaseFileUpdateHandler
    {
        private readonly List<IUpdateFilesBehavior> _updateFilesBehaviorList;
        private readonly IGetInstallationDirectories _getDirectoriesBehavior;
        private readonly ToscaConfigFilesModel _config;

        public BaseFileUpdateHandler(IGetInstallationDirectories getToscaServerDirectories, ToscaConfigFilesModel config)
        {
            _updateFilesBehaviorList = new List<IUpdateFilesBehavior>();
            _getDirectoriesBehavior = getToscaServerDirectories;
            _config = config;
        }

        public int GetAppCount()
        {
            return _updateFilesBehaviorList.Count;
        }

        public int GetUpdatedAppsCount()
        {
            int count = 0;
            foreach (var appUpdated in _updateFilesBehaviorList)
            {
                if (appUpdated.Updated)
                {
                    count++;
                }
            }
            return count;
        }

        public void ClearUpdateBehaviorList()
        {
            _updateFilesBehaviorList.Clear();
        }

        public int GetUpdatedFilesCount()
        {
            var count = 0;
            foreach (var updater in _updateFilesBehaviorList)
            {
                count += updater.UpdatedFilesCount;
            }
            return count;
        }

        public bool UpdateSucceeded()
        {
            foreach (var updater in _updateFilesBehaviorList)
            {
                if (!updater.Updated)
                {
                    return false; 
                }
            }
            return true;
        }

        public void AddUpdateBehavior(IUpdateFilesBehavior updateBehavior)
        {
            if (updateBehavior != null){
                _updateFilesBehaviorList.Add(updateBehavior);
            }
        }

        public void UpdateAll()
        {
            foreach (var updater in _updateFilesBehaviorList)
            {
                updater.Update(_config);
            }
        }

        public IEnumerable<string> GetInstalledAppPaths(string appPath)
        {
            return _getDirectoriesBehavior.GetAppPathList(appPath);
        }

        public void LoadAppUpdaterBehaviorList(string componentPath, IUpdateSettingsFactory usf)
        {
            Trace.WriteLine($"\nLoading files from {componentPath}.");
            Trace.WriteLine("============================");
            var installedApps = GetInstalledAppPaths(componentPath).ToList();
            foreach (var appPath in installedApps)
            {
                //create update behavior based on App Folder name
                var updater = usf.TryCreate(appPath);
                if (updater != null)
                {
                    AddUpdateBehavior(updater);
                }
            }
        }
    }
}
