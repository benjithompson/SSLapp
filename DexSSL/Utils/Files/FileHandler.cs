using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SSLapp.ViewModels;
using System.Diagnostics;

namespace SSLapp.Utils.Files
{
    class FileHandler
    {
        public static bool UpdateFiles(string serverpath)
        {
            if (string.IsNullOrEmpty(serverpath))
            {
                return false;
            }

            try
            {
                Console.WriteLine("UpdateFiles called.");
                var serverappspaths = Directory.EnumerateDirectories(serverpath);
                foreach (var serverapppath in serverappspaths)
                {

                    var appsettings = Directory.EnumerateFiles(serverapppath, "appsettings.json");
                    foreach (var appsetting in appsettings)
                    {
                        //UpdateJSONFiles.UpdateFile(appsetting, ToscaConfigFilesViewModel.ToscaConfigFiles);
                        //BaseFileUpdateHandler updateFiles = new BaseFileUpdateHandler(new );
                    }

                    //var webconfigs = Directory.EnumerateFiles(serverapppath, "*.config");
                    //foreach (var webconfig in webconfigs)
                    //{
                    //    //open file and replace values
                    //   // UpdateXMLFiles.UpdateFile(webconfig, ToscaConfigFilesViewModel.ToscaConfigFiles);
                    //}

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public static bool BackupFiles(string backuppath, string serverpath)
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
            backuppath = backuppath + @"\Tosca Server";
            try
            {
                Directory.CreateDirectory(backuppath);
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
                        File.Copy(appsetting, appfolder + appname, true);
                    }

                    var webconfigs = Directory.EnumerateFiles(serverapppath, "*.config");
                    foreach (var webconfig in webconfigs)
                    {
                        var webconfigname = Path.GetFileName(webconfig);
                        File.Copy(webconfig, appfolder + webconfigname, true);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

    }
}
