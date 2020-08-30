using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows;

namespace SSLapp.Utils.Services
{
    public abstract class BaseServiceRestarter
    {
        public abstract void RestartComponentServices();

        public void StartService(string serviceName, int timeoutMilliseconds = 15000)
        {
            ServiceController sc = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == serviceName);
            if (sc != null)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                    if (!(sc.Status.Equals(ServiceControllerStatus.Running)) &&
                        !(sc.Status.Equals(ServiceControllerStatus.StartPending)))
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }
                }
                catch
                {
                    Trace.WriteLine("Service " + serviceName + " failed to stop.");
                    MessageBox.Show(serviceName + " failed to start.", "Service Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void StopService(ServiceController sc, int timeoutMilliseconds = 15000)
        {
            try
            {
                
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }

            }
            catch
            {
                Trace.WriteLine("Service " + sc.ServiceName + " failed to stop.");
            }
        }

        public void StopService(string serviceName, int timeoutMilliseconds = 15000)
        {
            ServiceController sc = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == serviceName);
            if (sc != null)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                    if (!(sc.Status.Equals(ServiceControllerStatus.Stopped)) &&
                        !(sc.Status.Equals(ServiceControllerStatus.StopPending)))
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    }
                }
                catch
                {
                    Trace.WriteLine("Service " + serviceName + " failed to stop.");
                }
            }
        }

        public void RestartService(string serviceName, int timeoutMilliseconds)
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
    }
}
