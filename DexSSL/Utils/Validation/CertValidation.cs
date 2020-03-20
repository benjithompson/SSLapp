using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DexSSL.Utils
{
    static class CertValidation
    {

        public static X509Certificate2 SearchCertStore(string thumbprint = " ")
        {

            X509Store store;
            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly);
            var c = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false).OfType<X509Certificate2>().FirstOrDefault();
            store.Close();
            if (c != null)
                return c;
            else
            {
                store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.OpenExistingOnly);
                c = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false).OfType<X509Certificate2>().FirstOrDefault();
                store.Close();
                return c;
            }

        }

        public static bool CertificateIsFound(string thumbprint)
        {
            var c = SearchCertStore(thumbprint);

            return c != null;
        }

        public static string GetCertificateIssuedTo(string thumbprint)
        {
            var c = SearchCertStore(thumbprint);
            if(c == null){
                return String.Empty;
            }
            else
            {
                return c.GetNameInfo(X509NameType.SimpleName, false);
            }
        }   
    }
}
