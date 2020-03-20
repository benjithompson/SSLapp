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
        private string issuedTo;

        #region Constructor

        public ToscaConfigFilesModel()
        {
            serverPath = @"C:\Program Files (x86)\TRICENTIS\Tosca Server";
            outputConfigPath = @"C:\Temp";
            dexServerPort = "";
            NotifyPropertyChanged(nameof(ServerPath));
            NotifyPropertyChanged(nameof(OutputConfigPath));
            NotifyPropertyChanged(nameof(DexServerPort));
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
                        return "Enter valid hostname";
                    }
                    if(hostname != issuedTo)
                    {
                        return "Hostname doesn't match certificate";
                    }
                }
                if (propertyName == "DexServerPort")
                {
                    
                    if (string.IsNullOrEmpty(dexServerPort)){
                        return result;
                    }
                    var isNumeric = int.TryParse(dexServerPort, out int n);
                    if (!isNumeric)
                    {
                        result = "Must be valid port";
                    }
                }
                if (propertyName == "CertThumbprint")
                {
                    if (string.IsNullOrEmpty(certThumbprint))
                    {
                        return "Enter Thumbprint";
                    }
                    if (!CertThumbprintIsValid(certThumbprint))
                    {
                        result = "Enter valid thumbprint";
                    }
                    else if (!CertificateIsFound(certThumbprint))
                    {
                        result = "Cert not found";
                    }else
                    {
                        
                        issuedTo = GetCertificateIssuedTo(CertThumbprint);

                        if (String.IsNullOrEmpty(issuedTo)) 
                        {
                            return "Cert 'Issued To' empty";
                        }
                        else
                        {
                            Hostname = issuedTo;
                        }
                    }
                }
                return result;
            }
        }

        #endregion
    }
}
