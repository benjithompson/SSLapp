using DexSSL.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace DexSSL.ViewModels
{
    internal class ToscaConfigFilesViewModel
    {

        private ToscaConfigFiles _ToscaConfigFiles;

        //Constructor
        public ToscaConfigFilesViewModel()
        {
            _ToscaConfigFiles = new ToscaConfigFiles();
        }

        public ToscaConfigFiles ToscaConfigFiles
        {
            get
            {
                return _ToscaConfigFiles;
            }
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => ApplyConfig(), () => CanExecute));
            }
        }

        public static bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        public void ApplyConfig()
        {
            Debug.Assert(false, "ApplyConfig called");
        }
    }
}
