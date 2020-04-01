using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;

namespace SSLapp.Utils.Files.Update
{
    interface IUpdateFilesBehavior
    {
        public string AppPath { get; set; }
        public bool Updated { get; }
        public int UpdatedFilesCount { get; }
        public void Update(ToscaConfigFilesModel config);
    }
}
