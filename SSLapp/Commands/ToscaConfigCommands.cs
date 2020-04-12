using System.Diagnostics;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using Ookii.Dialogs.Wpf;
using System.Linq;
using SSLapp.Views;
using SSLapp.Utils.Files.Backups;

namespace SSLapp.Commands
{
    class ToscaConfigCommands
    {
        public static void UpdateToscaServerFiles(string serverpath)
        {
            IUpdateFilesBehavior updater = null;

            Trace.WriteLine("Updating Tosca Server Files.");
            Trace.WriteLine("============================\n");
            BaseFileUpdateHandler fileUpdateHandler = new BaseFileUpdateHandler(new GetToscaAppsBehavior(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            var installedApps = fileUpdateHandler.GetInstalledToscaApps(serverpath).ToList();
            UpdateSettingsFactory updateFactory = new UpdateSettingsFactory();
            foreach (var appPath in installedApps)
            {
                //create update behavior based on App Folder name
                updater = updateFactory.TryCreate(appPath);
                if (updater != null)
                {
                    fileUpdateHandler.AddUpdateBehavior(updater);

                }
            }
            fileUpdateHandler.UpdateAll();
            Trace.WriteLine($"Updated {fileUpdateHandler.GetUpdatedAppsCount()} directories.");
            if (fileUpdateHandler.UpdateSucceeded())
            {
                ToscaConfigFilesViewModel.ToscaConfigFiles.ApplyServerButton = "👍";
            }
            RestartServerWindow(fileUpdateHandler);
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

        public static void BackupTricentisSettings()
        {
            IBackupToscaFiles backup = new BackupToscaFiles(ToscaConfigFilesViewModel.ToscaConfigFiles.BackupPath);
            var serverCompleted = backup.BackupFiles(ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath);
            var agentCompleted = backup.BackupFiles(ToscaConfigFilesViewModel.ToscaConfigFiles.AgentPath);
            if (serverCompleted)
            {
                Trace.WriteLine($"Tosca Server settings files backed up to {backup.GetTarget()}");
            }
            if (agentCompleted)
            {
                Trace.WriteLine($"Distribution Agent settings backed up to {backup.GetTarget()}");
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

        public static void RestartServerWindow(BaseFileUpdateHandler updatehandler)
        {
            var vm = new UpdateCompleteViewModel(updatehandler);
            var UpdateWindow = new UpdateCompleteView { DataContext = vm };
            vm.OnRequestClose += (s, e) => UpdateWindow.Close();
            UpdateWindow.Owner = Application.Current.MainWindow;
            UpdateWindow.ShowDialog();
        }

    }
}
