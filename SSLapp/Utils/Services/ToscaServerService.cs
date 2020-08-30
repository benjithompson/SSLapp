using System;
using System.Collections.Generic;
using System.Text;

namespace SSLapp.Utils.Services
{
    class ToscaServerService
    {
        public string _name { get; set; }
        public string _status { get; set; }
        public bool _installed { get; set; }

        public ToscaServerService(string name)
        {
            _name = name;
            _status = "";
            _installed = false;
        }
    }
}
