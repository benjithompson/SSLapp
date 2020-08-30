using SSLapp.ViewModels;
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
    class ToscaServerServiceRestarter : BaseServiceRestarter
    {
        int restartCount = 0;
        List<ToscaServerService> toscaServiceList;

        public ToscaServerServiceRestarter(List<ToscaServerService> serviceList)
        {
            toscaServiceList = serviceList;
            initServiceStatus(serviceList);
        }

        private void initServiceStatus(List<ToscaServerService> tsl)
        {

            foreach (var toscaService in tsl)
            {
                ServiceController ctl = ServiceController.GetServices()
                    .FirstOrDefault(s => s.ServiceName == toscaService._name);
                if (ctl == null)
                    Trace.WriteLine($"{toscaService._name} Not installed");
                else
                {
                    toscaService._installed = true;
                }
            }
        }

        public override void RestartComponentServices()
        {
            RestartToscaServerServices();
            RestartIIS();
        }

        private void RestartToscaServerServices()
        {
            BackgroundWorker worker_restartServices = new BackgroundWorker
            {
                WorkerReportsProgress = false
            };
            worker_restartServices.DoWork += RestartToscaServerServicesAsync;
            worker_restartServices.RunWorkerAsync();
        }

        private void RestartIIS()
        {
            BackgroundWorker worker_restartIIS = new BackgroundWorker
            {
                WorkerReportsProgress = false
            };
            worker_restartIIS.DoWork += DoAsyncIISReset;
            worker_restartIIS.RunWorkerAsync();
        }

        private void RestartToscaServerServicesAsync(object sender, DoWorkEventArgs e)
        {
            //TODO: Invert dependancy (INotify?)
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Empty;
            TimeSpan timeout = TimeSpan.FromMilliseconds(15000);

            for(int i = toscaServiceList.Count-1; i >= 0; i-- )
            {
                StopService(toscaServiceList[i]._name);
            }

            //TODO: Remove and make notifier
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Tosca Server Services stopped.");

            foreach (var ts in toscaServiceList)
            {
                StartService(ts._name);
                restartCount++;
                UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("Starting " + ts._name + "... ({0}/{1})", restartCount.ToString(), toscaServiceList.Count.ToString());
            }
            UpdateCompleteViewModel.UpdateCompleteModel.CloseButtonVisible = true;
            UpdateCompleteViewModel.UpdateCompleteModel.CloseButton = "OK";
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = "Tricentis Service restart complete!";
        }

        private void DoAsyncIISReset(object o, DoWorkEventArgs args)
        {
            Trace.WriteLine("Restarting IIS...");
            using (Process iisReset = new Process())
            {

                iisReset.StartInfo.FileName = "iisreset.exe";
                iisReset.StartInfo.RedirectStandardOutput = false;
                iisReset.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                iisReset.Start();
                iisReset.WaitForExit();
            }
            Trace.WriteLine("Resetting IIS Complete!");
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Format("IIS Restarted");
        }
    }
}
