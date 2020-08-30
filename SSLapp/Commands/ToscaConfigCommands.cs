using System.Diagnostics;
using System.Windows;
using SSLapp.Utils.Files;
using SSLapp.Utils.Files.Update;
using SSLapp.ViewModels;
using Ookii.Dialogs.Wpf;
using System.Linq;
using SSLapp.Views;
using SSLapp.Utils.Files.Backups;
using SSLapp.Utils.Files.Updates;

namespace SSLapp.Commands
{
    class ToscaConfigCommands
    {

        public static void UpdateToscaServerFiles(string serverpath)
        {
            BaseFileUpdateHandler updater = new BaseFileUpdateHandler(new ToscaInstallation(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            updater.LoadAppUpdaterBehaviorList(serverpath, new UpdateToscaServerSettingsFactory());
            updater.UpdateAll();
            if (updater.UpdateSucceeded())
            {
                Trace.WriteLine("Update process complete.");
                ToscaConfigFilesViewModel.ToscaConfigFiles.ApplyServerButton = "👍";
                
            }
            else
            {
                //TODO:handle failed update
            }
            RestartServerWindow(updater);
        }

        public static void UpdateAgentFiles(string agentPath)
        {
            BaseFileUpdateHandler updater = new BaseFileUpdateHandler(new ToscaInstallation(), ToscaConfigFilesViewModel.ToscaConfigFiles);
            updater.LoadAppUpdaterBehaviorList(agentPath, new UpdateDexAgentSettingsFactory());
            updater.UpdateAll();
            Trace.WriteLine("Update process complete.");
            if (updater.UpdateSucceeded())
            {
                Trace.WriteLine("Update process complete.");
                ToscaConfigFilesViewModel.ToscaConfigFiles.ApplyAgentButton = "👍";
            }
        }

        public static void OpenServerDirectory(string path)
        {
            Trace.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog
            {
                SelectedPath = path
            };
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Trace.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath = selectedPath;
        }

        public static void OpenBackupDirectory(string path)
        {
            Trace.WriteLine("Open Directory " + path);
            VistaFolderBrowserDialog fd = new VistaFolderBrowserDialog
            {
                SelectedPath = path
            };
            fd.ShowDialog();
            var selectedPath = fd.SelectedPath;
            Trace.WriteLine("Selected Path: " + selectedPath);
            ToscaConfigFilesViewModel.ToscaConfigFiles.BackupPath = selectedPath;
        }

        public static void BackupTricentisSettings()
        {
            IBackupToscaFiles backup = new BackupToscaFiles(ToscaConfigFilesViewModel.ToscaConfigFiles.BackupPath);
            var serverCompleted = backup.BackupFiles(ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath);
            var agentCompleted = backup.BackupFiles(ToscaConfigFilesViewModel.ToscaConfigFiles.TestSuitePath);
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
