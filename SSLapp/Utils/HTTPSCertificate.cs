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

        public void SetCertificateWithThumbprint(string thumbprint)
        {

            X509Store RootStore = new X509Store("Root", StoreLocation.LocalMachine);
            RootStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection rootcollection = RootStore.Certificates;
            X509Certificate2Collection rootfcollection = rootcollection.Find(X509FindType.FindByThumbprint, thumbprint, true);
            X509Certificate2Collection rootscollection = X509Certificate2UI.SelectFromCollection(rootfcollection, "Certificate Select", "Verify Certificate inforamtion and click Ok.", X509SelectionFlag.SingleSelection);
            Console.WriteLine("Certificate Thumbprint selected: {0}{1}", rootscollection[0].Thumbprint, Environment.NewLine);
            RootStore.Close();
            RootStore.Dispose();
            

            if (rootscollection.Count == 1)
            {
                _thumbprint = rootscollection[0].Thumbprint;
                _certStoreLocation = StoreLocation.LocalMachine.ToString();
                _certStoreName = StoreName.Root.ToString();
                _certIssuedTo = rootscollection[0].GetNameInfo(X509NameType.SimpleName, false);
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
