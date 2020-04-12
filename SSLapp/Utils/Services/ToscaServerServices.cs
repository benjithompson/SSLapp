using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using SSLapp.ViewModels;
using System.Windows;

namespace SSLapp.Utils.Services
{
    static class ToscaServerServices
    {

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                Trace.WriteLine("Service " + serviceName + " failed to start.");
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch
            {
                Trace.WriteLine("Service " + serviceName + " failed to stop.");
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                Trace.WriteLine("Service " + serviceName + " failed to restart.");
            }
        }

        public static void RestartToscaServerServicesAsync(object sender, DoWorkEventArgs e)
        {

            List<ServiceController> scList = ServiceController.GetServices().Where(s => s.ServiceName.ToLower().StartsWith("tricentis")).ToList();
            TimeSpan timeout = TimeSpan.FromMilliseconds(15000);
            int restartCount = 0;
            int totalServices = scList.Count;

            foreach (ServiceController sc in scList)
            {
                UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Stopping Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                try
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Trace.Write(sc.ServiceName + " stopping... ");
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                        Trace.WriteLine("Done!");
                        restartCount++;
                    }
                }
                catch (Exception)
                {

                    Trace.WriteLine(sc.ServiceName + " already stopped.");
                }
            }

            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Stopping Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
            restartCount = 0;
            ServiceController service = null;

            try
            {
                service = scList.FirstOrDefault(s => s.ServiceName == "Tricentis.ServiceDiscovery");
                if (service != null)
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    restartCount++;
                    UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Starting " + service.ServiceName + "... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                    scList.Remove(service);
                }
                service = scList.FirstOrDefault(s => s.ServiceName == "Tricentis.AuthenticationService");
                if (service != null)
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    restartCount++;
                    UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Starting " + service.ServiceName + "... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                    scList.Remove(service);
                }
                service = scList.FirstOrDefault(s => s.ServiceName == "Tricentis.ProjectService");
                if (service != null)
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    restartCount++;
                    UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Starting " + service.ServiceName + "... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                    scList.Remove(service);
                }

            }
            catch (Exception)
            {
                Trace.WriteLine("Service " + service.ServiceName + " failed for some reason.");
            }

            foreach (var sc in scList)
            {
                UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Starting " + sc.ServiceName + "... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                try
                {
                    Trace.Write(sc.ServiceName + " starting... ");
                    sc.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    restartCount++;

                }
                catch (Exception)
                {

                    Trace.WriteLine(sc.ServiceName + " failed to start.");
                    MessageBox.Show(sc.ServiceName + " failed to start.", "Service Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            UpdateCompleteViewModel.UpdateCompleteModel.CloseButtonVisible = true;
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = "Tricentis Service restart complete!";
        }

        public static void DoAsyncIISReset(object o, DoWorkEventArgs args)
        {
            Trace.WriteLine("Resetting IIS...");
            using (Process iisReset = new Process())
            {

                iisReset.StartInfo.FileName = "iisreset.exe";
                iisReset.StartInfo.RedirectStandardOutput = false;
                iisReset.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                iisReset.Start();
                iisReset.WaitForExit();
            }
            Trace.WriteLine("Resetting IIS Complete!");
        }
    }
}
