using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using SSLapp.ViewModels;

namespace SSLapp.Utils.Executables
{
    class ExecutableHelpers
    {
        public static void RestartExe(string processName, string exePath)
        {
            if (IsExeRunning(processName))
            {
                exePath = GetProcessExePath(processName);
                StopProcessByName(processName);
                StartExe(exePath);
            }
            else
            {
                StartExe(exePath);
            }
        }

        public static void StartExe(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    BackgroundWorker worker_StartProcess = new BackgroundWorker();
                    worker_StartProcess.WorkerReportsProgress = false;
                    worker_StartProcess.DoWork += StartExeAsyc;
                    worker_StartProcess.RunWorkerAsync(argument: path);
                }
                catch (Exception)
                {
                    Trace.WriteLine("Exception while trying to start " + path);
                    throw;
                }
            }
        }

        public static void StopProcessByName(string exeName)
        {
            try
            {
                foreach (var proc in Process.GetProcessesByName(exeName))
                {
                    Trace.WriteLine("Killing " + proc.ProcessName);
                    proc.Kill();
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception when trying to kill DistributionAgent process.");
            }
        }

        public static bool IsExeRunning(string processName)
        {
            var process = Process.GetProcessesByName(processName);
            return (process.Length > 0) ? true : false;
        }

        private static void StartExeAsyc(object e, DoWorkEventArgs args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = args.Argument.ToString();
            proc.Start();
            proc.WaitForExit();
            
        }

        public static string GetProcessExePath(string processName)
        {
            var exePath = string.Empty;
            try
            {
                foreach (var proc in Process.GetProcessesByName(processName))
                {
                    exePath = proc.MainModule.FileName;
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception when getting process exe path");
            }
            return exePath;
        }
    }
}
