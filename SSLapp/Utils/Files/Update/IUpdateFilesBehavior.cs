using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;

namespace SSLapp.Utils.Files.Update
{
    interface IUpdateFilesBehavior
    {
        public void Update(string directoryPath, ToscaConfigFilesModel config);

    }
}
