using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SSLapp.ViewModels;
using System.Diagnostics;

namespace SSLapp.Utils.Files
{
    class BackupFiles
    {

        public static bool BackupToscaServerFiles(string backuppath, string serverpath)
        {
            if (string.IsNullOrEmpty(backuppath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(serverpath))
            {
                return false;
            }

            if (backuppath.EndsWith(@"\"))
            {
                backuppath = backuppath.Remove(backuppath.Length - 1, 1);
            }

            //create Tosca Server folder in BackupPath
            try
            {
                Directory.CreateDirectory(backuppath);
                var serverappspaths = Directory.EnumerateDirectories(serverpath);
                foreach (var serverapppath in serverappspaths)
                {

                    var foldername = Path.GetFileName(serverapppath);
                    Trace.WriteLine("Backing up " + foldername);
                    var appfolder = backuppath + @"\" + foldername + @"\";
                    Directory.CreateDirectory(appfolder);

                    var appsettings = Directory.EnumerateFiles(serverapppath, "appsettings.json");
                    foreach (var appsetting in appsettings)
                    {
                        var appname = Path.GetFileName(appsetting);
                        File.Copy(appsetting, appfolder + appname, true);
                    }

                    var webconfigs = Directory.EnumerateFiles(serverapppath, "*web.config");
                    foreach (var webconfig in webconfigs)
                    {
                        var webconfigname = Path.GetFileName(webconfig);
                        File.Copy(webconfig, appfolder + webconfigname, true);
                    }
                    var execonfigs = Directory.EnumerateFiles(serverapppath, "*exe.config");
                    foreach (var execonfig in execonfigs)
                    {
                        var webconfigname = Path.GetFileName(execonfig);
                        File.Copy(execonfig, appfolder + webconfigname, true);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return false;
            }
            return true;
        }

    }
}
