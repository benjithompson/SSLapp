using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using Ookii.Dialogs.Wpf;
using System.IO;


namespace SSLapp.Commands
{
    class Commands
    {
        public static void UpdateToscaServerFiles(string serverpath)
        {
            Console.WriteLine("Updating Tosca Server Files.");
            Console.WriteLine("============================\n");
            BaseFileUpdateHandler updateHandler = new BaseFileUpdateHandler(new GetDirectoriesBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var serverApps = updateHandler.GetToscaServerDirectories(serverpath);
            foreach (var serverApp in serverApps)
            {
                updateHandler.AddFileUpdateBehavior(serverApp);
            }
            updateHandler.UpdateAll();
            Console.WriteLine("Updating files complete!");
            //TODO: Prompt Update Complete.
            //TODO: confirm restart of services:

        }

        public static void OpenServerDirectory(string path)
        {
            Console.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog();
            fd.SelectedPath = path;
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Console.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath = selectedPath;
        }

        public static void OpenBackupDirectory(string path)
        {
            Console.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog();
            fd.SelectedPath = path;
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Console.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.OutputConfigPath = selectedPath;
        }

        public static void BackupToscaServerSettings(string backuppath, string serverpath)
        {
            var completed = BackupFiles.BackupToscaServerFiles(backuppath, serverpath);
            if (completed)
            {
                ToscaConfigFilesViewModel.ToscaConfigFiles.BackupState = "Done!";
                //TODO: dialog to show complete
                MessageBox.Show("Tosca Server Appsettings and Web.config files backed up to " + backuppath, "Backup Complete");
            }
            
        }
    }
}
