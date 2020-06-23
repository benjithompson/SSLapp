using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Diagnostics;

namespace SSLapp.Utils
{
    public class IISCertificateHelpers
    {
        private string _thumbprint;
        private string _certIssuedTo;
        private string _certStoreName;
        private string _certStoreLocation;

        public IISCertificateHelpers(){}

        public IISCertificateHelpers(string thumbprint)
        {
            SetCertificateWithThumbprint(thumbprint);
        }

        public string GetCertificateThumbprint() => _thumbprint;
        public string GetCertificateStoreName() => _certStoreName;
        public string GetCertificateStoreLocation() => _certStoreLocation;
        public string GetCertIssuedTo() => _certIssuedTo;
        public void SetCertificateWithThumbprint(string thumbprint)
        {
            try
            {
                X509Certificate2Collection rootfcollection = GetCertificate(thumbprint);
                if (rootfcollection.Count > 1)
                {
                    rootfcollection = X509Certificate2UI.SelectFromCollection(rootfcollection, "Certificate Select", "Verify Certificate inforamtion and click Ok.", X509SelectionFlag.SingleSelection);
                }
                Trace.WriteLine(String.Format("Certificate selected. Hostname found: {0}", rootfcollection[0].FriendlyName));

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

        public X509Certificate2Collection GetCertificate(string thumbprint, string storeLocation = "Root")
        {
            Trace.WriteLine("Searching for " + thumbprint + " in " + storeLocation + " Certificate Store.");
            X509Store RootStore = new X509Store(storeLocation, StoreLocation.LocalMachine);
            RootStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection rootcollection = RootStore.Certificates;
            RootStore.Close();
            X509Certificate2Collection certCollection = rootcollection.Find(X509FindType.FindByThumbprint, thumbprint, true);
            Trace.WriteLine("Found " + certCollection.Count + " certificate in store.");
            return certCollection;
        }

        public bool CertificateFound(string thumbprint, string storeLocation="Root")
        {
            if(GetCertificate(thumbprint).Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
