using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SSLapp.ViewModels;
using System.ComponentModel;

using System.Linq;
using SSLapp.Models;
using SSLapp.Utils.Executables;
using SSLapp.Utils.Services;

namespace SSLapp.Commands
{
    class UpdateCompleteCommands
    {


        public static void RestartToscaServerDependencies()
        {
            Trace.WriteLine("Restarting Tosca Server Dependencies:");
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Empty;
            UpdateCompleteViewModel.UpdateCompleteModel.AcceptButtonVisible = false;
            UpdateCompleteViewModel.UpdateCompleteModel.DeclineButtonVisible = false;
            UpdateCompleteViewModel.UpdateCompleteModel.CloseButtonVisible = true;

            BackgroundWorker worker_restartIIS = new BackgroundWorker();
            worker_restartIIS.WorkerReportsProgress = false;
            worker_restartIIS.DoWork += ToscaServerServices.DoAsyncIISReset;
            worker_restartIIS.RunWorkerAsync();

            BackgroundWorker worker_restartServices = new BackgroundWorker();
            worker_restartServices.WorkerReportsProgress = false;
            worker_restartServices.DoWork += ToscaServerServices.RestartToscaServerServicesAsync;
            worker_restartServices.RunWorkerAsync();

            ExecutableHelpers.RestartExe("RdpServer", ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath + @"\DEXRdpServer\ToscaRdpServer.exe");

            UpdateCompleteViewModel.GetUpdateHandler().ResetUpdateApps();
        }

    }
}
