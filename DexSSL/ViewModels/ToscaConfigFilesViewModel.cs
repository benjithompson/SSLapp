using SSLapp.Commands;
using SSLapp.Models;
using System.Windows.Input;

namespace SSLapp.ViewModels
{
    public class ToscaConfigFilesViewModel
    {

        private static ToscaConfigFilesModel _ToscaConfigFilesModel;
        private ICommand _applyCommand;
        private ICommand _openServerPathCommand;
        private ICommand _backupToPathCommand;

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
                if (string.IsNullOrEmpty(_ToscaConfigFilesModel.Hostname) || string.IsNullOrEmpty(_ToscaConfigFilesModel.CertThumbprint))
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
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                if (string.IsNullOrEmpty(_ToscaConfigFilesModel.ServerPath) || string.IsNullOrEmpty(_ToscaConfigFilesModel.OutputConfigPath))
                {
                    return false;
                }
                return true;
            }
        }

        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new CommandHandler(() => Commands.Commands.ApplyConfig(_ToscaConfigFilesModel.ServerPath), () => CanExecuteApply));
            }
        }

        public ICommand OpenServerPath
        {
            get
            {
                return _openServerPathCommand ?? (_openServerPathCommand = new CommandHandler(() => Commands.Commands.OpenDirectory(_ToscaConfigFilesModel.ServerPath), () => true));
            }
        }

        public ICommand BackupToPath
        {
            get
            {
                return _backupToPathCommand ?? (_backupToPathCommand = new CommandHandler(() => Commands.Commands.BackupDirectory(_ToscaConfigFilesModel.OutputConfigPath, _ToscaConfigFilesModel.ServerPath), () => CanExecuteBackup));
            }
        }
    }
}
