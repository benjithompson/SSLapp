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

            X509Store RootStore = new X509Store("Root", StoreLocation.LocalMachine);
            RootStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection rootcollection = RootStore.Certificates;
            X509Certificate2Collection rootfcollection = rootcollection.Find(X509FindType.FindByThumbprint, thumbprint, true);
            X509Certificate2Collection rootscollection = X509Certificate2UI.SelectFromCollection(rootfcollection, "Certificate Select", "Verify Certificate inforamtion and click Ok.", X509SelectionFlag.SingleSelection);
            Console.WriteLine("Number of certificates: {0}{1}", rootscollection.Count, Environment.NewLine);
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

        //    X509Store MyStore = new X509Store("MY", StoreLocation.LocalMachine);
        //    MyStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        //    X509Certificate2Collection collection = MyStore.Certificates;
        //    X509Certificate2Collection fcollection = collection.Find(X509FindType.FindByThumbprint, thumbprint, true);
        //    X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Certificate Select", "Verify Certificate inforamtion and click Ok.", X509SelectionFlag.SingleSelection);
        //    Console.WriteLine("Number of certificates: {0}{1}", scollection.Count, Environment.NewLine);
        //    MyStore.Close();
        //    MyStore.Dispose();

        //    if (scollection.Count == 1)
        //    {
        //        _thumbprint = scollection[0].Thumbprint;
        //        _certStoreLocation = StoreLocation.LocalMachine.ToString();
        //        _certStoreName = StoreName.Root.ToString();
        //        _certIssuedTo = scollection[0].GetNameInfo(X509NameType.SimpleName, false);
        //        _certValid = true;
        //        return;
        //    }

        }

        public string GetCertificateThumbprint() => _thumbprint;

        public string GetCertificateStoreName() => _certStoreName;
        public string GetCertificateStoreLocation() => _certStoreLocation;
        public string GetCertIssuedTo() => _certIssuedTo;
        public bool CertificateIsValid(string thumbprint) => _certValid;
    }
}
