using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using SSLapp.Commands;
using SSLapp.Models;
using SSLapp.Views;
using System.ServiceProcess;
using System.Linq;

namespace SSLapp.ViewModels
{
    class UpdateCompleteViewModel
    {
        private static UpdateCompleteModel _updateCompleteModel;
        private ICommand _accept_command;
        private ICommand _decline_command;
        public event EventHandler OnRequestClose;

        public UpdateCompleteViewModel()
        {
            _updateCompleteModel = new UpdateCompleteModel();
        }

        public static UpdateCompleteModel UpdateCompleteModel => _updateCompleteModel;

        public ICommand AcceptCommand
        {
            get
            {
                return _accept_command ?? (_accept_command = new CommandHandler(() => RestartServices(), () => true));
            }
        }

        public ICommand DeclineCommand
        {
            get
            {
                return _decline_command ?? (_decline_command = new CommandHandler(() => CloseUpdateCompleteWindow(), () => true));
            }
        }

        public void CloseUpdateCompleteWindow()
        {
            OnRequestClose(this, new EventArgs());
        }

        public void RestartServices()
        {
            Trace.WriteLine("Restarting Tosca Server Services:");
            List<ServiceController> scList = ServiceController.GetServices().Where(s => s.ServiceName.ToLower().StartsWith("tricentis")).ToList();
            TimeSpan timeout = TimeSpan.FromMilliseconds(15000);
            foreach (ServiceController sc in scList)
            {
                try
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        Trace.Write(sc.ServiceName + " stopping... ");
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                        Trace.WriteLine("Done!");
                    }
                }
                catch (Exception)
                {

                    Trace.WriteLine(sc.ServiceName + " already stopped.");
                }
            }

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
                    scList.Remove(service);
                }
                service = scList.FirstOrDefault(s => s.ServiceName == "Tricentis.AuthenticationService");
                if (service != null)
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    scList.Remove(service);
                }
                service = scList.FirstOrDefault(s => s.ServiceName == "Tricentis.ProjectService");
                if (service != null)
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    scList.Remove(service);
                }

            }
            catch (Exception)
            {
                Trace.WriteLine("Service " + service.ServiceName + " failed for some reason.");
            }

            foreach (var sc in scList)
            {
                try
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    sc.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");

                }
                catch (Exception)
                {

                    Trace.WriteLine(sc.ServiceName + " failed to start.");
                    throw;
                }

            }

            CloseUpdateCompleteWindow();
        }
    }
}
