using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SSLapp.Utils
{
    static class FieldValidations
    {
        public static bool CertThumbprintIsValid(string cert)
        {
            if(cert is null) { return false; }

            Regex rx = new Regex("(?<nospace>([0-9a-f]{40}))|(?<space>([0-9a-f]{2} ){19}([0-9a-f]{2}))");
            MatchCollection matches = rx.Matches(cert);

            // Report the number of matches found.
            Console.WriteLine("{0} matches found in:\n   {1}",
                              matches.Count,
                              cert);
            return matches.Count == 1 && cert == matches[0].Value;
        }
    }
}
