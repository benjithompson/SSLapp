using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SSLapp.Utils
{
    public class HTTPSCertificate
    {
        private string _thumbprint;
        private string _certIssuedTo;
        private string _certStoreName;
        private string _certStoreLocation;
        private bool _certValid;

        public HTTPSCertificate(){}

        public void SetCertificate(string thumbprint)
        {

            X509Store store;
            store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly);
            var c = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false).OfType<X509Certificate2>().FirstOrDefault();
            store.Close();
            if (c != null)
            {
                _thumbprint = thumbprint;
                _certStoreLocation = StoreLocation.LocalMachine.ToString();
                _certStoreName = StoreName.Root.ToString();
                _certIssuedTo = c.GetNameInfo(X509NameType.SimpleName, false);
                _certValid = true;
                return;
            }
            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly);
            c = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false).OfType<X509Certificate2>().FirstOrDefault();
            store.Close();
            if (c != null)
            {
                _thumbprint = thumbprint;
                _certStoreLocation = StoreLocation.LocalMachine.ToString();
                _certStoreName = StoreName.My.ToString();
                _certIssuedTo = c.GetNameInfo(X509NameType.SimpleName, false);
                _certValid = true;
            }
        }
        public string GetCertificateThumbprint() => _thumbprint;

        public string GetCertificateStoreName() => _certStoreName;
        public string GetCertificateStoreLocation() => _certStoreLocation;
        public string GetCertIssuedTo() => _certIssuedTo;
        public bool CertificateIsValid(string thumbprint) => _certValid;
    }
}
