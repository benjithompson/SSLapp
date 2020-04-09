using SSLapp.Commands;
using SSLapp.Models;
using System.Windows.Input;
using System.ComponentModel;

namespace SSLapp.ViewModels
{
    public class ToscaConfigFilesViewModel
    {
        private static ToscaConfigFilesModel _ToscaConfigFilesModel;
        private ICommand _applyServerCommand;
        private ICommand _applyAgentCommand;
        private ICommand _openServerPathCommand;
        private ICommand _openAgentPathCommand;
        private ICommand _backupToPathCommand;
        private ICommand _openBackupPathCommand;
        private ICommand _restartServerCommand;
        private ICommand _restartAgentCommand;

        //Constructor
        public ToscaConfigFilesViewModel()
        {
            _ToscaConfigFilesModel = new ToscaConfigFilesModel();
        }

        public static ToscaConfigFilesModel ToscaConfigFiles => _ToscaConfigFilesModel;
        public static bool CanExecuteServerApply
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
        public static bool CanExecuteAgentApply
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                if (string.IsNullOrEmpty(_ToscaConfigFilesModel.AgentPath))
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

        public ICommand ApplyServerCommand
        {
            get
            {
                return _applyServerCommand ?? (_applyServerCommand = new CommandHandler(() 
                    => Commands.ToscaConfigCommands.UpdateToscaServerFiles(_ToscaConfigFilesModel.ServerPath), () => CanExecuteServerApply));
            }
        }
        public ICommand ApplyAgentCommand
        {
            get
            {
                return _applyAgentCommand ?? (_applyAgentCommand = new CommandHandler(()
                    => Commands.ToscaConfigCommands.UpdateAgentFiles(_ToscaConfigFilesModel.AgentPath), () => CanExecuteAgentApply));
            }
        }
        public ICommand OpenServerPath
        {
            get
            {
                return _openServerPathCommand ?? (_openServerPathCommand = new CommandHandler(() => Commands.ToscaConfigCommands.OpenServerDirectory(_ToscaConfigFilesModel.ServerPath), () => true));
            }
        }
        public ICommand OpenAgentPath
        {
            get
            {
                return _openAgentPathCommand ?? (_openAgentPathCommand = new CommandHandler(() => Commands.ToscaConfigCommands.OpenServerDirectory(_ToscaConfigFilesModel.AgentPath), () => true));
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
        public ICommand RestartServerCommand
        {
            get
            {
                return _restartServerCommand ?? (_restartServerCommand = new CommandHandler(() => ToscaConfigCommands.RestartServerWindow(), () => true));
            }
        }
        public ICommand RestartAgentCommand
        {
            get
            {
                return _restartAgentCommand ?? (_restartAgentCommand = new CommandHandler(() => ToscaConfigCommands.RestartAgent(_ToscaConfigFilesModel.AgentPath), () => true));
            }
        }
        #endregion
    }
}
