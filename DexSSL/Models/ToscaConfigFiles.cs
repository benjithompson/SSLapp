using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using static DexSSL.Utils.FieldValidations;


namespace DexSSL
{
    public class ToscaConfigFiles : IDataErrorInfo, INotifyPropertyChanged
    {


        private string outputConfigPath;
        private string dexServerHostName;
        private string dexServerPort;
        private string certThumbprint;
        public bool HasErrors = true;

        #region Constructor

        public ToscaConfigFiles()
        {

        }

        #endregion

        #region Properties

        public string OutputConfigPath
        {
            get { return outputConfigPath; }
            set
            {
                outputConfigPath = value;
                NotifyPropertyChanged(nameof(OutputConfigPath));
            }
        }
        public string DexServerHostName
        {
            get { return dexServerHostName; }
            set
            {
                dexServerHostName = value;
                NotifyPropertyChanged(nameof(DexServerHostName));
            }
        }
        public string DexServerPort
        {
            get { return dexServerPort; }
            set
            {
                dexServerPort = value;
                NotifyPropertyChanged(nameof(DexServerPort));
            }
        }
        public string CertThumbprint
        {
            get { return certThumbprint; }
            set
            {
                certThumbprint = value;
                NotifyPropertyChanged(nameof(CertThumbprint));
            }
        }



        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get;
            private set;
        }


        /// IDataErrorInfo
        public string this[string propertyName]{
            get { 
                string result = String.Empty;
                if (propertyName == "OutputConfigPath")
                {
                    if (string.IsNullOrEmpty(OutputConfigPath))
                    {
                        result = "Output path must not be empty";
                    }
                    else if(!Directory.Exists(OutputConfigPath))
                    {
                        result = "Directory does not exist";
                    }
                }
                if (propertyName == "DexServerHostName")
                {
                    if (string.IsNullOrEmpty(dexServerHostName))
                    {
                        result = " ";
                    }
                }
                if (propertyName == "DexServerPort")
                {
                    var isNumeric = int.TryParse(dexServerPort, out int n);
                    if (string.IsNullOrEmpty(dexServerPort) || !isNumeric)
                    {
                        result = " ";
                    }
                }
                if (propertyName == "CertThumbprint")
                {
                    if (!CertThumbprintIsValid(certThumbprint))
                    {
                        result = " ";
                    }
                }
                if(String.IsNullOrEmpty(result))
                {
                    HasErrors = false;
                }
                return result;
            }
        }

        #endregion
    }
}
