using DexSSL.Commands;
using System.Windows.Input;

namespace DexSSL.ViewModels
{
    internal class ToscaConfigFilesViewModel
    {

        private static ToscaConfigFilesModel _ToscaConfigFiles;
        private ICommand _applyCommand;
        private ICommand _openServerPathCommand;
        private ICommand _backupToPathCommand;

        //Constructor
        public ToscaConfigFilesViewModel()
        {
            _ToscaConfigFiles = new ToscaConfigFilesModel();
        }

        public static ToscaConfigFilesModel ToscaConfigFiles => _ToscaConfigFiles;

        public static bool CanExecuteApply
        {

            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                if (string.IsNullOrEmpty(_ToscaConfigFiles.Hostname) || string.IsNullOrEmpty(_ToscaConfigFiles.CertThumbprint))
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
                if (string.IsNullOrEmpty(_ToscaConfigFiles.ServerPath) || string.IsNullOrEmpty(_ToscaConfigFiles.OutputConfigPath))
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
                return _applyCommand ?? (_applyCommand = new CommandHandler(() => Commands.Commands.ApplyConfig(_ToscaConfigFiles.ServerPath), () => CanExecuteApply));
            }
        }

        public ICommand OpenServerPath
        {
            get
            {
                return _openServerPathCommand ?? (_openServerPathCommand = new CommandHandler(() => Commands.Commands.OpenDirectory(_ToscaConfigFiles.ServerPath), () => true));
            }
        }

        public ICommand BackupToPath
        {
            get
            {
                return _backupToPathCommand ?? (_backupToPathCommand = new CommandHandler(() => Commands.Commands.BackupDirectory(_ToscaConfigFiles.OutputConfigPath, _ToscaConfigFiles.ServerPath), () => CanExecuteBackup));
            }
        }


    }
}
