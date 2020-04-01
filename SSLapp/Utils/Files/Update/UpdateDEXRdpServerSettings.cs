using SSLapp.Models;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateDEXRdpServerSettings : IUpdateFilesBehavior
    {
        public UpdateDEXRdpServerSettings(string appPath)
        {
            AppPath = appPath;
        }
        public string AppPath { get ; set; }
        public bool Updated => throw new System.NotImplementedException();
        public int UpdatedFilesCount => throw new System.NotImplementedException();
        public void Update(ToscaConfigFilesModel config)
        {
            Trace.WriteLine("RdpServer not implemented");
        }
    }
}
