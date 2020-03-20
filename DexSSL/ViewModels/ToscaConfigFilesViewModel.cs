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

        public static bool CanExecute
        {

            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new CommandHandler(() => Commands.Commands.ApplyConfig(_ToscaConfigFiles.ServerPath), () => CanExecute));
            }
        }

        public ICommand OpenServerPath
        {
            get
            {
                return _openServerPathCommand ?? (_openServerPathCommand = new CommandHandler(() => Commands.Commands.OpenDirectory(_ToscaConfigFiles.ServerPath), () => CanExecute));
            }
        }

        public ICommand BackupToPath
        {
            get
            {
                return _backupToPathCommand ?? (_backupToPathCommand = new CommandHandler(() => Commands.Commands.OpenDirectory(_ToscaConfigFiles.OutputConfigPath), () => CanExecute));
            }
        }


    }
}
