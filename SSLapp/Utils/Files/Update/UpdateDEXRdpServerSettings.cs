using SSLapp.Models;
using System.Diagnostics;

namespace SSLapp.Utils.Files.Update
{
    class UpdateDEXRdpServerSettings : IUpdateFilesBehavior
    {
        public void Update(string filepath, ToscaConfigFilesModel config)
        {
            Debug.WriteLine("RdpServer not implemented");
        }
    }
}
