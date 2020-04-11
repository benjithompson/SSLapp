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
using System.ComponentModel;
using System.IO;

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
                return _accept_command ?? (_accept_command = new CommandHandler(() => RestartServicesAsync(), () => true));
            }
        }

        public ICommand DeclineCommand
        {
            get
            {
                return _decline_command ?? (_decline_command = new CommandHandler(() => CloseUpdateCompleteWindow(), () => true));
            }
        }

        public ICommand CloseCommand
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

        public void RestartServicesAsync()
        {
            Trace.WriteLine("Restarting Tosca Server Services:");
            _updateCompleteModel.AcceptButtonVisible = false;
            _updateCompleteModel.DeclineButtonVisible = false;
            BackgroundWorker worker_restartServices = new BackgroundWorker();
            worker_restartServices.WorkerReportsProgress = false;
            worker_restartServices.DoWork += startAsyncserviceRestart;
            worker_restartServices.RunWorkerAsync();

            BackgroundWorker worker_restartIIS = new BackgroundWorker();
            worker_restartIIS.WorkerReportsProgress = false;
            worker_restartIIS.DoWork += UpdateCompleteCommands.DoAsyncIISReset;
            worker_restartIIS.RunWorkerAsync();

            RestartRdpServer();

        }

         void startAsyncserviceRestart(object sender, DoWorkEventArgs e)
        {

            List<ServiceController> scList = ServiceController.GetServices().Where(s => s.ServiceName.ToLower().StartsWith("tricentis")).ToList();
            TimeSpan timeout = TimeSpan.FromMilliseconds(15000);
            int restartCount = 0;
            int totalServices = scList.Count;

            

            foreach (ServiceController sc in scList)
            {
                _updateCompleteModel.TextBlockMessage = string.Format("Stopping Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
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

            _updateCompleteModel.TextBlockMessage = string.Format("Stopping Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
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
                    _updateCompleteModel.TextBlockMessage = string.Format("Starting Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
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
                    _updateCompleteModel.TextBlockMessage = string.Format("Starting Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
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
                    _updateCompleteModel.TextBlockMessage = string.Format("Starting Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                    scList.Remove(service);
                }

            }
            catch (Exception)
            {
                Trace.WriteLine("Service " + service.ServiceName + " failed for some reason.");
            }

            foreach (var sc in scList)
            {
                _updateCompleteModel.TextBlockMessage = string.Format("Starting Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
                try
                {
                    Trace.Write(service.ServiceName + " starting... ");
                    sc.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    Trace.WriteLine("Done!");
                    restartCount++;

                }
                catch (Exception)
                {

                    Trace.WriteLine(sc.ServiceName + " failed to start.");
                    throw;
                }

            }
            _updateCompleteModel.TextBlockMessage = string.Format("Starting Services... ({0}/{1})", restartCount.ToString(), totalServices.ToString());
            _updateCompleteModel.CloseButtonVisible = true;
            _updateCompleteModel.TextBlockMessage = "Tricentis Service restart complete!";
        }

        public static void RestartRdpServer()
        {
            var rdpPath = string.Empty;
            try
            {
                foreach (var proc in Process.GetProcessesByName("ToscaRdpServer"))
                {
                    rdpPath = proc.MainModule.FileName;
                    Trace.WriteLine("Killing " + proc.ProcessName);
                    proc.Kill();
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception when trying to kill DistributionAgent process.");
            }

            if (File.Exists(rdpPath))
            {
                try
                {
                    BackgroundWorker worker_restartAgent = new BackgroundWorker();
                    worker_restartAgent.WorkerReportsProgress = false;
                    worker_restartAgent.DoWork += StartRdpServerAsync;
                    worker_restartAgent.RunWorkerAsync(argument: rdpPath);

                }
                catch (Exception)
                {
                    Trace.WriteLine("Exception while trying to start " + rdpPath);
                    throw;
                }
            }
        }

        private static void StartRdpServerAsync(object sender, DoWorkEventArgs args)
        {
            Process.Start((string)args.Argument);
        }
    }
}
