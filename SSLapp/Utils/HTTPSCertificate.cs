using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Diagnostics;

namespace SSLapp.Utils
{
    public class HTTPSCertificate
    {
        private string _thumbprint;
        private string _certIssuedTo;
        private string _certStoreName;
        private string _certStoreLocation;

        public HTTPSCertificate(){}

        public void SetCertificateWithThumbprint(string thumbprint)
        {
            try
            {
                Trace.WriteLine("Searching for " + thumbprint + " in Root Certificate Store.");
                X509Store RootStore = new X509Store("Root", StoreLocation.LocalMachine);
                RootStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509Certificate2Collection rootcollection = RootStore.Certificates;
                X509Certificate2Collection rootfcollection = rootcollection.Find(X509FindType.FindByThumbprint, thumbprint, true);
                Trace.WriteLine("Found " + rootfcollection.Count + " certificate in store.");
                if (rootfcollection.Count > 1)
                {
                    rootfcollection = X509Certificate2UI.SelectFromCollection(rootfcollection, "Certificate Select", "Verify Certificate inforamtion and click Ok.", X509SelectionFlag.SingleSelection);
                }
                Trace.WriteLine("Certificate selected: {0}{1}", rootfcollection[0].FriendlyName);
                RootStore.Close();
                RootStore.Dispose();


                if (rootfcollection.Count == 1)
                {
                    _thumbprint = rootfcollection[0].Thumbprint;
                    _certStoreLocation = StoreLocation.LocalMachine.ToString();
                    _certStoreName = StoreName.Root.ToString();
                    _certIssuedTo = rootfcollection[0].GetNameInfo(X509NameType.SimpleName, false);
                }
            }
            catch (Exception)
            {

                Trace.WriteLine("Certificate Exeception.");
            }

        }

        public bool CertificateFound(string thumbprint)
        {
            X509Store RootStore = new X509Store("Root", StoreLocation.LocalMachine);
            RootStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection rootcollection = RootStore.Certificates;
            X509Certificate2Collection rootfcollection = rootcollection.Find(X509FindType.FindByThumbprint, thumbprint, true);

            if(rootfcollection.Count > 0)
            {
                return true;
            }
            return false;
        }

        public string GetCertificateThumbprint() => _thumbprint;
        public string GetCertificateStoreName() => _certStoreName;
        public string GetCertificateStoreLocation() => _certStoreLocation;
        public string GetCertIssuedTo() => _certIssuedTo;
    }
}
