using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using System.IO;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class BaseFileUpdateHandler
    {
        List<IUpdateFilesBehavior> _updateFilesBehaviorList = new List<IUpdateFilesBehavior>();
        IGetToscaApps _getDirectoriesBehavior;
        ToscaConfigFilesModel _config;

        public int GetUpdatedAppsCount()
        {
            return _updateFilesBehaviorList.Count;
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

        public BaseFileUpdateHandler(ToscaConfigFilesModel config)
        {
            _config = config;
        }

        public BaseFileUpdateHandler(IGetToscaApps getToscaServerDirectories, ToscaConfigFilesModel config)
        {
            _getDirectoriesBehavior = getToscaServerDirectories;
            _config = config;
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

        public IEnumerable<string> GetInstalledToscaApps(string appPath)
        {
            return _getDirectoriesBehavior.GetToscaApps(appPath);
        }
    }
}
