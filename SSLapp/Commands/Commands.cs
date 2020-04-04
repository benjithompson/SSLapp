using System.Diagnostics;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using Ookii.Dialogs.Wpf;
using System;
using System.Linq;
using SSLapp.Views;

namespace SSLapp.Commands
{
    class Commands
    {
        public static void UpdateToscaServerFiles(string serverpath)
        {
            Trace.WriteLine("Updating Tosca Server Files.");
            Trace.WriteLine("============================\n");
            BaseFileUpdateHandler fileUpdateHandler = new BaseFileUpdateHandler(new GetDirectoriesBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var installedApps = fileUpdateHandler.GetInstalledToscaServerApps(serverpath).ToList();
            //create factory
            UpdateSettingsFactory updateFactory = new UpdateSettingsFactory();
            var count = 0;
            foreach (var appPath in installedApps)
            {
                //create update behavior based on App Folder name
                var updater = updateFactory.TryCreate(appPath);
                if (updater != null)
                {
                    fileUpdateHandler.AddUpdateBehavior(updater);
                    count++;
                }
            }
            fileUpdateHandler.UpdateAll();
            Trace.WriteLine("Update process complete.");
            if (fileUpdateHandler.UpdateSucceeded())
            {
                ToscaConfigFilesViewModel.ToscaConfigFiles.AppliedState = "👍";
            }

            var vm = new UpdateCompleteViewModel();
            var UpdateWindow = new UpdateCompleteView
            {
                DataContext = vm
            };
            vm.OnRequestClose += (s, e) => UpdateWindow.Close();
            UpdateWindow.ShowDialog();

            //TODO: confirm restart of services:
        }

        public static void OpenServerDirectory(string path)
        {
            Trace.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog();
            fd.SelectedPath = path;
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Trace.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath = selectedPath;
        }

        public static void OpenBackupDirectory(string path)
        {
            Trace.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog();
            fd.SelectedPath = path;
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Trace.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.BackupPath = selectedPath;
        }

        public static void BackupToscaServerSettings(string backuppath, string serverpath)
        {
            var completed = BackupFiles.BackupToscaServerFiles(backuppath, serverpath);
            if (completed)
            {
                ToscaConfigFilesViewModel.ToscaConfigFiles.BackupState = "Done!";
                //TODO: dialog to show complete
            }
            
        }

        public static void AcceptCommand()
        {
            Trace.WriteLine("AcceptCommand called");
        }

        public static void DeclineCommand(Window UpdateCompleteWindow)
        {
            Trace.WriteLine("DeclineCommand called");
            UpdateCompleteWindow.Close();
        }
    }
}
