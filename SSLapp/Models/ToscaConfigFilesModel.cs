using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using static SSLapp.Utils.FieldValidations;
using SSLapp.Utils;
using System.Diagnostics;

namespace SSLapp.Models
{
    public class ToscaConfigFilesModel : IDataErrorInfo, INotifyPropertyChanged
    {

        private string _serverPath;
        private string _backupPath;
        private string _hostname;
        private string _dexServerPort;
        private string certThumbprint;
        private string _backupState;
        private string _appliedState;
        private HTTPSCertificate _httpCert = new HTTPSCertificate();
       

        #region Constructor

        public ToscaConfigFilesModel()
        {
            ServerPath = @"C:\Program Files (x86)\TRICENTIS\Tosca Server";
            BackupPath = @"C:\Temp";
            DexServerPort = "";
            BackupState = "Backup";
            AppliedState = "Apply";
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
                BackupState = "Backup";
                AppliedState = "Apply";
            }
        }
        public string BackupPath
        {
            get { return _backupPath; }
            set
            {
                _backupPath = value;
                NotifyPropertyChanged(nameof(BackupPath));
                BackupState = "Backup";
            }
        }
        public string Hostname
        {
            get { return _hostname; }
            set
            {
                _hostname = value;
                NotifyPropertyChanged(nameof(Hostname));
                AppliedState = "Apply";
            }
        }
        public string DexServerPort
        {
            get { return _dexServerPort; }
            set
            {
                _dexServerPort = value;
                NotifyPropertyChanged(nameof(DexServerPort));
                AppliedState = "Apply";
            }
        }
        public string CertThumbprint
        {
            get { return certThumbprint; }
            set
            {
                certThumbprint = value;
                NotifyPropertyChanged(nameof(CertThumbprint));
                AppliedState = "Apply";
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
        public string AppliedState
        {
            get { return _appliedState; }
            set
            {
                _appliedState = value;
                NotifyPropertyChanged(nameof(AppliedState));
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
                    result = ServerPathValidation();
                }
                if (propertyName == "OutputConfigPath")
                {
                    result = BackupPathValidation();
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

                    if (!_httpCert.CertificateFound(certThumbprint))
                    {
                        result = "Cert not found";
                    }else
                    {
                        _httpCert.SetCertificateWithThumbprint(certThumbprint);
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

        public string BackupPathValidation()
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(BackupPath))
            {
                result = "Backup path must not be empty";
            }
            else if (!Directory.Exists(BackupPath))
            {
                result = "Directory does not exist";
            }
            return result;
        }

        public string ServerPathValidation()
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(ServerPath))
            {
                result = "Server path must not be empty";
            }
            else if (!Directory.Exists(ServerPath))
            {
                result = "Directory does not exist";
            }
            return result;
        }

        public bool BackupValid()
        {
            
            var isValid = string.IsNullOrEmpty(BackupPathValidation()) && string.IsNullOrEmpty(ServerPathValidation());
            return isValid;
        }

        #endregion
    }
}
