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

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        public void SaveChanges()
        {
            Debug.Assert(false, string.Format("{0} was updated", ToscaConfigFiles.OutputConfigPath));
            Debug.Assert(false, string.Format("{0} was updated", ToscaConfigFiles.DexServerHostName));
            Debug.Assert(false, string.Format("{0} was updated", ToscaConfigFiles.DexServerPort));
            Debug.Assert(false, string.Format("{0} was updated", ToscaConfigFiles.CertThumbprint));
        }
    }
}
