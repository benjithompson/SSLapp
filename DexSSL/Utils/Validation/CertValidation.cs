using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DexSSL.Utils
{
    static class CertValidation
    {

        public static bool SearchCertStore(string thumbprint, StoreName storeName, StoreLocation storeLocation)
        {
            X509Store store;
            store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.OpenExistingOnly);
            var c = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false).OfType<X509Certificate>().FirstOrDefault();
            store.Close();
            return c != null;
        }

        public static bool CertificateIsFound(string thumbprint)
        {
            return SearchCertStore(thumbprint, StoreName.My, StoreLocation.LocalMachine) || SearchCertStore(thumbprint, StoreName.Root, StoreLocation.LocalMachine);
        }
                
    }
}
