using System;
using System.Collections.Generic;
using System.Text;

namespace DexSSL.Utils.Files
{
    class HandleFiles
    {

        public static void CopyFileToDirectory(string fromDir, string toDir)
        {
            if (string.IsNullOrEmpty(fromDir))
            {
                throw new ArgumentException("message", nameof(fromDir));
            }

            if (string.IsNullOrEmpty(toDir))
            {
                throw new ArgumentException("message", nameof(toDir));
            }
            //copy from tosca server to working dir

        }

        public static void UpdateFiles(string filepath)
        {
            if (filepath is null)
            {
                throw new ArgumentNullException(nameof(filepath));
            }
        }

        public static void BackupFiles(string filepath)
        {

        }

    }
}
