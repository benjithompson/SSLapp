using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using static DexSSL.Utils.FieldValidations;
using static DexSSL.Utils.CertValidation;


namespace DexSSL
{
    public class ToscaConfigFilesModel : IDataErrorInfo, INotifyPropertyChanged
    {

        private string serverPath;
        private string outputConfigPath;
        private string hostname;
        private string dexServerPort;
        private string certThumbprint;

        #region Constructor

        public ToscaConfigFilesModel()
        {

        }

        #endregion

        #region Properties

        public string ServerPath
        {
            get { return serverPath; }
            set
            {
                serverPath = value;
                NotifyPropertyChanged(nameof(ServerPath));
            }
        }
        public string OutputConfigPath
        {
            get { return outputConfigPath; }
            set
            {
                outputConfigPath = value;
                NotifyPropertyChanged(nameof(OutputConfigPath));
            }
        }
        public string Hostname
        {
            get { return hostname; }
            set
            {
                hostname = value;
                NotifyPropertyChanged(nameof(Hostname));
                NotifyPropertyChanged(nameof(CertThumbprint));
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
                NotifyPropertyChanged(nameof(Hostname));
            }
        }

        public string DefaultServerPath
        {
            get { return @"C:\Program Files (x86)\TRICENTIS\Tosca Server\";}
        }
        public string DefaultBackupPath
        {
            get { return @"C:\temp\"; }
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
                if (propertyName == "ServerPath")
                {
                    if (string.IsNullOrEmpty(ServerPath))
                    {
                        result = "Server path must not be empty";
                    }
                    else if (!Directory.Exists(ServerPath))
                    {
                        result = "Directory does not exist";
                    }
                }
                if (propertyName == "OutputConfigPath")
                {
                    if (string.IsNullOrEmpty(OutputConfigPath))
                    {
                        result = "Backup path must not be empty";
                    }
                    else if(!Directory.Exists(OutputConfigPath))
                    {
                        result = "Directory does not exist";
                    }
                }
                if (propertyName == "Hostname")
                {
                    if (string.IsNullOrEmpty(Hostname))
                    {
                        result = "Enter valid hostname";
                    }
                }
                if (propertyName == "DexServerPort")
                {
                    var isNumeric = int.TryParse(dexServerPort, out int n);
                    if (string.IsNullOrEmpty(dexServerPort) || !isNumeric)
                    {
                        result = "Must be valid port";
                    }
                }
                if (propertyName == "CertThumbprint")
                {
                    if (!CertThumbprintIsValid(certThumbprint))
                    {
                        result = "Invalid Thumbprint";
                    }
                    else if (!CertificateIsFound(certThumbprint))
                    {
                        result = "Cert not found";
                    }else
                    {
                        
                        var issuedTo = GetCertificateIssuedTo(CertThumbprint);
                        if (String.IsNullOrEmpty(Hostname)) 
                        {
                            return "Can't compare empty hostname";
                        }
                        if (issuedTo.ToLower() != Hostname.ToLower())
                        {
                            result = "cert issueto doesn't match hostname provided";
                        }
                    }
                    

                }
                return result;
            }
        }

        #endregion
    }
}
