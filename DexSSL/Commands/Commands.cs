using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using System.IO;


namespace SSLapp.Commands
{
    class Commands
    {
        public static void UpdateToscaServerFiles(string serverpath)
        {
            Console.WriteLine("UpdateToscaServerFiles called.");
            BaseFileUpdateHandler updater = new BaseFileUpdateHandler(new UpdateAuthServiceAppsettings(), new GetDirectoriesBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var serverApps = updater.GetToscaServerDirectories(serverpath);
            foreach (var serverApp in serverApps)
            {
                updater.Update(serverApp);
            }
        }

        public static void OpenDirectory(string path)
        {
            Debug.Assert(false, "OpenDirectory called with" + path);
        }

        public static void BackupDirectory(string backuppath, string serverpath)
        {

            var completed = FileHandler.BackupFiles(backuppath, serverpath);
            if (completed)
            {
                ToscaConfigFilesViewModel.ToscaConfigFiles.BackupState = "Done!";
            }
            
        }
    }
}
