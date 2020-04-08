using SSLapp.Commands;
using SSLapp.Models;
using System.Windows.Input;
using System.ComponentModel;

namespace SSLapp.ViewModels
{
    public class ToscaConfigFilesViewModel
    {
        private static ToscaConfigFilesModel _ToscaConfigFilesModel;
        private ICommand _applyCommand;
        private ICommand _openServerPathCommand;
        private ICommand _backupToPathCommand;
        private ICommand _openBackupPathCommand;
        private ICommand _restartServicesCommand;

        //Constructor
        public ToscaConfigFilesViewModel()
        {
            _ToscaConfigFilesModel = new ToscaConfigFilesModel();
        }

        public static ToscaConfigFilesModel ToscaConfigFiles => _ToscaConfigFilesModel;
        public static bool CanExecuteApply
        {

            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                if (string.IsNullOrEmpty(_ToscaConfigFilesModel.Hostname) || string.IsNullOrEmpty(_ToscaConfigFilesModel.GetCertificate.GetCertificateThumbprint()) || !string.IsNullOrEmpty(_ToscaConfigFilesModel.ServerPathValidation()))
                {
                    return false;
                }
                return true;
            }
        }
        public static bool CanExecuteBackup
        {
            get
            {
                return _ToscaConfigFilesModel.BackupValid();
            }
        }

        #region Commands

        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new CommandHandler(() 
                    => Commands.ToscaConfigCommands.UpdateToscaServerFiles(_ToscaConfigFilesModel.ServerPath), () => CanExecuteApply));
            }
        }
        public ICommand OpenServerPath
        {
            get
            {
                return _openServerPathCommand ?? (_openServerPathCommand = new CommandHandler(() => Commands.ToscaConfigCommands.OpenServerDirectory(_ToscaConfigFilesModel.ServerPath), () => true));
            }
        }
        public ICommand OpenBackupPath
        {
            get
            {
                return _openBackupPathCommand ?? (_openBackupPathCommand = new CommandHandler(() => Commands.ToscaConfigCommands.OpenBackupDirectory(_ToscaConfigFilesModel.BackupPath), () => true));
            }
        }
        public ICommand BackupToPath
        {
            get
            {
                return _backupToPathCommand ?? (_backupToPathCommand = new CommandHandler(() => Commands.ToscaConfigCommands.BackupToscaServerSettings(_ToscaConfigFilesModel.BackupPath, _ToscaConfigFilesModel.ServerPath), () => CanExecuteBackup));
            }
        }
        public ICommand RestartCommand
        {
            get
            {
                return _restartServicesCommand ?? (_restartServicesCommand = new CommandHandler(() => ToscaConfigCommands.RestartServicesWindow(), () => true));
            }
        }
        #endregion
    }
}
