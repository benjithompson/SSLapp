using SSLapp.Commands;
using SSLapp.Models;
using System.Windows.Input;
using SSLapp.Utils.Executables;

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
                if (string.IsNullOrEmpty(_ToscaConfigFilesModel.TestSuitePath) || string.IsNullOrEmpty(_ToscaConfigFilesModel.Hostname))
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
                ICommand command = _applyServerCommand ??= new CommandHandler(()
                    => Commands.ToscaConfigCommands.UpdateToscaServerFiles(_ToscaConfigFilesModel.ServerPath), () => CanExecuteServerApply);
                return command;
            }
        }
        public ICommand ApplyAgentCommand
        {
            get
            {
                ICommand command  = _applyAgentCommand ??= new CommandHandler(()
                    => Commands.ToscaConfigCommands.UpdateAgentFiles(_ToscaConfigFilesModel.TestSuitePath), () => CanExecuteAgentApply);
                return command;
            }
        }
        public ICommand OpenServerPath
        {
            get
            {
                ICommand command = _openServerPathCommand ??= new CommandHandler(() => ToscaConfigCommands.OpenServerDirectory(_ToscaConfigFilesModel.ServerPath), () => true);
                return command;
            }
        }
        public ICommand OpenAgentPath
        {
            get
            {
                ICommand command = _openAgentPathCommand ??= new CommandHandler(() => ToscaConfigCommands.OpenServerDirectory(_ToscaConfigFilesModel.TestSuitePath), () => true);
                return command;
            }
        }
        public ICommand OpenBackupPath
        {
            get
            {
                ICommand command = _openBackupPathCommand ??= new CommandHandler(() => ToscaConfigCommands.OpenBackupDirectory(_ToscaConfigFilesModel.BackupPath), () => true);
                return command;
            }
        }
        public ICommand BackupServerSettings
        {
            get
            {
                ICommand command = _backupToPathCommand ??= new CommandHandler(() => ToscaConfigCommands.BackupTricentisSettings(), () => CanExecuteBackup);
                return command;
            }
        }
        public ICommand RestartServerCommand
        {
            get
            {
                ICommand command = _restartServerCommand ??= new CommandHandler(() => ToscaConfigCommands.RestartServerWindow(), () => true);
                return command;
            }
        }
        public ICommand RestartAgentCommand
        {
            get
            {
                ICommand command = _restartAgentCommand ??= new CommandHandler(() => ExecutableHelpers.RestartExe("ToscaDistributionAgent", ToscaConfigFiles.TestSuitePath + @"\DistributedExecution\ToscaDistributionAgent.exe"), () => true);
                return command;
            }
        }
        #endregion
    }
}
