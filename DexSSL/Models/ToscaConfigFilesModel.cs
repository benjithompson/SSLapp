using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using static SSLapp.Utils.FieldValidations;
using SSLapp.Utils;


namespace SSLapp.Models
{
    public class ToscaConfigFilesModel : IDataErrorInfo, INotifyPropertyChanged
    {

        private string _serverPath;
        private string _outputConfigPath;
        private string _hostname;
        private string _dexServerPort;
        private string _backupState;
        private string certThumbprint;
        private HTTPSCertificate _httpCert = new HTTPSCertificate();
       

        #region Constructor

        public ToscaConfigFilesModel()
        {
            _serverPath = @"C:\Program Files (x86)\TRICENTIS\Tosca Server";
            _outputConfigPath = @"C:\Temp";
            _dexServerPort = "";
            _backupState = "Backup";

            NotifyPropertyChanged(nameof(ServerPath));
            NotifyPropertyChanged(nameof(OutputConfigPath));
            NotifyPropertyChanged(nameof(DexServerPort));
        }

        #endregion

        #region Properties

        public string ServerPath
        {
            get { return _serverPath; }
            set
            {
                _serverPath = value;
                NotifyPropertyChanged(nameof(ServerPath));
                _backupState = "Backup";
                NotifyPropertyChanged(nameof(BackupState));
            }
        }
        public string OutputConfigPath
        {
            get { return _outputConfigPath; }
            set
            {
                _outputConfigPath = value;
                NotifyPropertyChanged(nameof(OutputConfigPath));
                _backupState = "Backup";
                NotifyPropertyChanged(nameof(BackupState));
            }
        }
        public string Hostname
        {
            get { return _hostname; }
            set
            {
                _hostname = value;
                NotifyPropertyChanged(nameof(Hostname));
            }
        }
        public string DexServerPort
        {
            get { return _dexServerPort; }
            set
            {
                _dexServerPort = value;
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

        public HTTPSCertificate GetCertificate
        {
            get { return _httpCert; } 
        }

        public string DefaultServerPath
        {
            get { return @"C:\Program Files (x86)\TRICENTIS\Tosca Server\";}
        }
        public string DefaultBackupPath
        {
            get { return @"C:\temp\"; }
        }
        public string BackupState {
            get { return _backupState; }
            set
            {
                _backupState = value;
                NotifyPropertyChanged(nameof(BackupState));
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
                    if(_hostname != _httpCert.GetCertIssuedTo())
                    {
                        return "Hostname doesn't match certificate";
                    }
                }
                if (propertyName == "DexServerPort")
                {
                    
                    if (string.IsNullOrEmpty(_dexServerPort)){
                        return result;
                    }
                    var isNumeric = int.TryParse(_dexServerPort, out int n);
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
                        return "Enter valid thumbprint";
                    }

                    _httpCert.SetCertificate(certThumbprint);

                    if (!_httpCert.CertificateIsValid(certThumbprint))
                    {
                        result = "Cert not found";
                    }else
                    {

                        if (String.IsNullOrEmpty(_httpCert.GetCertIssuedTo())) 
                        {
                            return "Cert 'Issued To' empty";
                        }
                        else
                        {
                            Hostname = _httpCert.GetCertIssuedTo();
                        }
                    }
                }
                if (propertyName == "BackupState")
                {
                    //if (string.IsNullOrEmpty(backupState))
                    //{
                    //    result = "needs backup";
                    //}
                }
                return result;
            }
        }

        #endregion
    }
}
