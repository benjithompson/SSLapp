using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Text;

namespace DexSSL
{
    public class ToscaConfigFiles : IDataErrorInfo, INotifyPropertyChanged
    {


        private string outputConfigPath;
        private string dexServerHostName;
        private string dexServerPort;
        private string certThumbprint;

        #region Constructor

        public ToscaConfigFiles()
        {
            preloadConfig();

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
                dexServerPort = value;
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
                }
                if (propertyName == "DexServerHostName")
                {
                    if (string.IsNullOrEmpty(dexServerHostName))
                    {
                        result = "Hostname must not be empty. Use Full Computer Name.";
                    }
                }
                if (propertyName == "DexServerPort")
                {
                    var isNumeric = int.TryParse(dexServerPort, out int n);
                    if (string.IsNullOrEmpty(dexServerPort) || !isNumeric)
                    {
                        result = "Port must be integer.";
                    }
                }

                return result;
            }
        }

        #endregion

        #region Private Methods
        private void preloadConfig()
        {
            if (Directory.Exists(@"C:\Users\"+Environment.UserName+@"\Desktop"))
            {
                OutputConfigPath = @"C:\Users\" + Environment.UserName + @"\Desktop\";
            }
            else
            {
                OutputConfigPath = @"C:\";
            }

            DexServerPort = "443";
        }
        #endregion
    }
}
