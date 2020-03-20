using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;


namespace DexSSL.Commands
{
    class Commands
    {
        public static void ApplyConfig(string serverpath)
        {
            Utils.Files.HandleFiles.UpdateFiles(serverpath);
        }

        public static void OpenDirectory(string path)
        {
            Debug.Assert(false, "OpenDirectory called with" + path);
        }

        public static void BackupDirectory(string path)
        {
            Utils.Files.HandleFiles.BackupFiles(path);
        }

    }
}
