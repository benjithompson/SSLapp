﻿using System.Diagnostics;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using Ookii.Dialogs.Wpf;


namespace SSLapp.Commands
{
    class Commands
    {
        public static void UpdateToscaServerFiles(string serverpath)
        {
            Debug.WriteLine("Updating Tosca Server Files.");
            Debug.WriteLine("============================\n");
            BaseFileUpdateHandler updateHandler = new BaseFileUpdateHandler(new GetDirectoriesBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var serverApps = updateHandler.GetToscaServerDirectories(serverpath);
            foreach (var serverApp in serverApps)
            {
                updateHandler.AddFileUpdateBehavior(serverApp);
            }
            updateHandler.UpdateAll();
            Debug.WriteLine("Updating files complete!");
            ToscaConfigFilesViewModel.ToscaConfigFiles.AppliedState = "👍";
            //TODO: Prompt Update Complete.
            //TODO: confirm restart of services:

        }

        public static void OpenServerDirectory(string path)
        {
            Debug.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog();
            fd.SelectedPath = path;
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Debug.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath = selectedPath;
        }

        public static void OpenBackupDirectory(string path)
        {
            Debug.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog();
            fd.SelectedPath = path;
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Debug.WriteLine("Selected Path: " + selectedPath);
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
    }
}
