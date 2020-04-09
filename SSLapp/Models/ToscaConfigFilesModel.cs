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
        private string _agentPath;
        private string _backupPath;
        private string _hostname;
        private string _dexServerPort;
        private string certThumbprint;
        private string _backupButton;
        private string _applyServerButton;
        private string _applyAgentButton;
        private string _restartServerButton;
        private string _restartAgentButton;
        private HTTPSCertificate _httpCert = new HTTPSCertificate();

        #region Constructor

        public ToscaConfigFilesModel()
        {
            ServerPath = @"C:\Program Files (x86)\TRICENTIS\Tosca Server";
            AgentPath = @"C:\Program Files (x86)\TRICENTIS\Tosca Testsuite";
            BackupPath = @"C:\Temp";
            DexServerPort = "";
            BackupButton = "Backup";
            ApplyServerButton = "Apply";
            ApplyAgentButton = "Apply";
            RestartServerButton = "Restart";
            RestartAgentButton = "Restart";
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
                BackupButton = "Backup";
                ApplyServerButton = "Apply";
            }
        }
        public string AgentPath
        {
            get { return _agentPath; }
            set
            {
                _agentPath = value;
                NotifyPropertyChanged(nameof(AgentPath));
                BackupButton = "Backup";
                ApplyAgentButton = "Apply";
            }
        }
        public string BackupPath
        {
            get { return _backupPath; }
            set
            {
                _backupPath = value;
                NotifyPropertyChanged(nameof(BackupPath));
                BackupButton = "Backup";
            }
        }
        public string Hostname
        {
            get { return _hostname; }
            set
            {
                _hostname = value;
                NotifyPropertyChanged(nameof(Hostname));
                ApplyServerButton = "Apply";
            }
        }
        public string DexServerPort
        {
            get { return _dexServerPort; }
            set
            {
                _dexServerPort = value;
                NotifyPropertyChanged(nameof(DexServerPort));
                ApplyServerButton = "Apply";
            }
        }
        public string CertThumbprint
        {
            get { return certThumbprint; }
            set
            {
                certThumbprint = value;
                NotifyPropertyChanged(nameof(CertThumbprint));
                ApplyServerButton = "Apply";
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
        public string BackupButton {
            get { return _backupButton; }
            set
            {
                _backupButton = value;
                NotifyPropertyChanged(nameof(BackupButton));
            } 
        }
        public string ApplyServerButton
        {
            get { return _applyServerButton; }
            set
            {
                _applyServerButton = value;
                NotifyPropertyChanged(nameof(ApplyServerButton));
            }
        }
        public string ApplyAgentButton
        {
            get { return _applyAgentButton; }
            set
            {
                _applyAgentButton = value;
                NotifyPropertyChanged(nameof(ApplyAgentButton));
            }
        }
        public string RestartServerButton
        {
            get { return _restartServerButton; }
            set
            {
                _restartServerButton = value;
                NotifyPropertyChanged(nameof(RestartServerButton));
            }
        }
        public string RestartAgentButton
        {
            get { return _restartAgentButton; }
            set
            {
                _restartAgentButton = value;
                NotifyPropertyChanged(nameof(RestartAgentButton));
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
                if (propertyName == "AgentPath")
                {
                    result = AgentPathValidation();
                }
                if (propertyName == "BackupPath")
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
                    return CertThumbprintValidation();
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

        public string AgentPathValidation()
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(AgentPath))
            {
                result = "Agent path must not be empty";
            }
            else if (!Directory.Exists(AgentPath))
            {
                result = "Directory does not exist";
            }
            return result;
        }

        public string CertThumbprintValidation()
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(certThumbprint))
            {
                return "Enter Thumbprint";
            }
            else if (!CertThumbprintIsValid(certThumbprint))
            {
                return "Enter valid thumbprint";
            }

            else if (!_httpCert.CertificateFound(certThumbprint))
            {
                result = "Cert not found in Trusted Root";
            }
            _httpCert.SetCertificateWithThumbprint(certThumbprint);
            Hostname = _httpCert.GetCertIssuedTo();

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
