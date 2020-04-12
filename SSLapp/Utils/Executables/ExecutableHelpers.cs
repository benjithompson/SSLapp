using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace SSLapp.Utils.Executables
{
    class ExecutableHelpers
    {
        public static void RestartExe(string processName)
        {
            if (IsExeRunning(processName))
            {
                var exePath = GetProcessExePath(processName);
                StopProcessByName(processName);
                StartExe(exePath);
            }
        }

        public static void StartExe(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    BackgroundWorker worker_restartAgent = new BackgroundWorker();
                    worker_restartAgent.WorkerReportsProgress = false;
                    worker_restartAgent.DoWork += StartExeAsyc;
                    worker_restartAgent.RunWorkerAsync(argument: path);
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
            return (Process.GetProcessesByName(processName) != null) ? true : false;
        }

        private static void StartExeAsyc(object e, DoWorkEventArgs args)
        {
            Process.Start((string)args.Argument);
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
