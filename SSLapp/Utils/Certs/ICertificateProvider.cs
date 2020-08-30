using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SSLapp.Utils.Certs
{
    interface ICertificateProvider
    {
        public string GetCertificateThumbprint();
        public string GetCertificateStoreName();
        public string GetCertificateStoreLocation();
        public string GetCertIssuedTo();
        public void SetCertificateWithThumbprint(string thumbprint);
        public X509Certificate2Collection GetCertificate(string thumbprint, string storeLocation = "Root");
        public bool CertificateFound(string thumbprint, string storeLocation = "Root");
    }
}
