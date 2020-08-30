using System;
using System.Collections.Generic;
using System.Diagnostics;
using SSLapp.ViewModels;
using SSLapp.Utils.Services;
using SSLapp.Utils.Executables;

namespace SSLapp.Commands
{
    class UpdateCompleteCommands
    {
        public static void RestartToscaServerDependencies()
        {
            List<ToscaServerService> toscaServerList = new List<ToscaServerService>()
            {
                new ToscaServerService("Tricentis.ServiceDiscovery"),
                new ToscaServerService("Tricentis.AuthenticationService"),
                new ToscaServerService("Tricentis.ProjectService"),
                new ToscaServerService("Tricentis.FileService"),
                new ToscaServerService("Tricentis.ToscaAutomationObjectService")
            };

            Trace.WriteLine("Restarting Tosca Server Dependencies:");
            UpdateCompleteViewModel.UpdateCompleteModel.TextBlockMessage = string.Empty;
            UpdateCompleteViewModel.UpdateCompleteModel.AcceptButtonVisible = false;
            UpdateCompleteViewModel.UpdateCompleteModel.DeclineButtonVisible = false;
            UpdateCompleteViewModel.UpdateCompleteModel.CloseButtonVisible = false;

            ToscaServerServiceRestarter serviceRestarter = new ToscaServerServiceRestarter(toscaServerList);
            serviceRestarter.RestartComponentServices();

            ExecutableHelpers.RestartExe("RdpServer", ToscaConfigFilesViewModel.ToscaConfigFiles.ServerPath + @"\DEXRdpServer\ToscaRdpServer.exe");

            //updatehandler is assigned when files are updated. If 'Restart' is used without updating first, updater will be null. This is shit code.
            if (UpdateCompleteViewModel.GetUpdateHandler() != null)
            {
                UpdateCompleteViewModel.GetUpdateHandler().ClearUpdateBehaviorList();
            }
        }
    }
}
