using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using DexSSL.Utils.Files;


namespace DexSSL.Commands
{
    class Commands
    {
        public static void ApplyConfig(string serverpath)
        {
            HandleFiles.UpdateFiles(serverpath);
        }

        public static void OpenDirectory(string path)
        {
            Debug.Assert(false, "OpenDirectory called with" + path);
        }

        public static void BackupDirectory(string backuppath, string serverpath)
        {
            HandleFiles.BackupFiles(backuppath, serverpath);
        }

    }
}
