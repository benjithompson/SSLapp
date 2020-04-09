using System.Diagnostics;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using Ookii.Dialogs.Wpf;
using System;
using System.Linq;
using SSLapp.Views;
using System.IO;
using System.ComponentModel;

namespace SSLapp.Commands
{
    class ToscaConfigCommands
    {
        public static void UpdateToscaServerFiles(string serverpath)
        {
            Trace.WriteLine("Updating Tosca Server Files.");
            Trace.WriteLine("============================\n");
            BaseFileUpdateHandler fileUpdateHandler = new BaseFileUpdateHandler(new GetToscaAppsBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var installedApps = fileUpdateHandler.GetInstalledToscaApps(serverpath).ToList();
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
                ToscaConfigFilesViewModel.ToscaConfigFiles.ApplyServerButton = "👍";
            }
            RestartServerWindow();
        }

        public static void UpdateAgentFiles(string agentPath)
        {
            Trace.WriteLine("Updating Execution Agent Files.");
            Trace.WriteLine("============================\n");
            BaseFileUpdateHandler fileUpdateHandler = new BaseFileUpdateHandler(new GetToscaAppsBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var installedApps = fileUpdateHandler.GetInstalledToscaApps(agentPath).ToList();
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
                ToscaConfigFilesViewModel.ToscaConfigFiles.ApplyAgentButton = "👍";
            }
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
                ToscaConfigFilesViewModel.ToscaConfigFiles.BackupButton = "Done!";
                //TODO: dialog to show complete
            }
            
        }
        public static void RestartServerWindow()
        {
            var vm = new UpdateCompleteViewModel();
            var UpdateWindow = new UpdateCompleteView { DataContext = vm };
            vm.OnRequestClose += (s, e) => UpdateWindow.Close();
            UpdateWindow.Owner = Application.Current.MainWindow;
            UpdateWindow.ShowDialog();
        }

        public static void RestartAgent(string toscaPath)
        {
            var agentPath = toscaPath + @"\DistributedExecution\ToscaDistributionAgent.exe";
            try
            {
                foreach (var proc in Process.GetProcessesByName("ToscaDistributionAgent")){
                    Trace.WriteLine("Killing " + proc.ProcessName);
                    proc.Kill();
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception when trying to kill DistributionAgent process.");
            }

            if(File.Exists(agentPath))
            {
                try
                {
                    BackgroundWorker worker_restartAgent = new BackgroundWorker();
                    worker_restartAgent.WorkerReportsProgress = false;
                    worker_restartAgent.DoWork += StartAgentAsyc;
                    worker_restartAgent.RunWorkerAsync(argument: agentPath);

                }
                catch (Exception)
                {
                    Trace.WriteLine("Exception while trying to start " + agentPath);
                    throw;
                }
            }
        }

        public static void StartAgentAsyc(object e, DoWorkEventArgs args)
        {
            Process.Start((string)args.Argument);
        }
    }
}
