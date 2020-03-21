using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using DexSSL.Utils.Files;
using DexSSL.ViewModels;


namespace DexSSL.Commands
{
    class Commands
    {
        public static void ApplyConfig(string serverpath)
        {
            HandleFiles.UpdateFiles(serverpath);
        }

        public static void OpenDirectory(string path)
        {
            Debug.Assert(false, "OpenDirectory called with" + path);
        }

        public static void BackupDirectory(string backuppath, string serverpath)
        {
            var completed = HandleFiles.BackupFiles(backuppath, serverpath);
            if (completed)
            {
                ToscaConfigFilesViewModel.ToscaConfigFiles.BackupState = "Done!";
            }
            
        }
    }
}
