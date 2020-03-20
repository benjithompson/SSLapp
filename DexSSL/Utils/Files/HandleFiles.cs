using System;
using System.IO;
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

        public static void BackupFiles(string backuppath, string serverpath)
        {
            if (string.IsNullOrEmpty(backuppath))
            {
                throw new ArgumentException("message", nameof(backuppath));
            }

            if (string.IsNullOrEmpty(serverpath))
            {
                throw new ArgumentException("message", nameof(serverpath));
            }

            if (backuppath.EndsWith(@"\"))
            {
                backuppath = backuppath.Remove(backuppath.Length - 1, 1);
            }

            //create Tosca Server folder in BackupPath
            backuppath = backuppath + @"\Tosca Server";
            Directory.CreateDirectory(backuppath);
            
            //open directory and find all files needed for configuration
            var serverappspaths = Directory.EnumerateDirectories(serverpath);
            foreach (var serverapppath in serverappspaths)
            {

                var foldername = Path.GetFileName(serverapppath);
                var appfolder = backuppath + @"\" + foldername + @"\";
                Directory.CreateDirectory(appfolder);

                var appsettings = Directory.EnumerateFiles(serverapppath, "appsettings.json");
                foreach (var appsetting in appsettings)
                {
                    var appname = Path.GetFileName(appsetting);
                    File.Copy(appsetting, appfolder+appname, true);
                }

                var webconfigs = Directory.EnumerateFiles(serverapppath, "*.config");
                foreach (var webconfig in webconfigs)
                {
                    var webconfigname = Path.GetFileName(webconfig);
                    File.Copy(webconfig, appfolder + webconfigname, true);
                }

            }
        }

    }
}
