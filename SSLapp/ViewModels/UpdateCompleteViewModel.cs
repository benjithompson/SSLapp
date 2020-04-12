using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using SSLapp.Commands;
using SSLapp.Models;
using SSLapp.Utils.Services;
using System.ComponentModel;

namespace SSLapp.ViewModels
{
    class UpdateCompleteViewModel 
    {
        private ICommand _accept_command;
        private ICommand _decline_command;
        public event EventHandler OnRequestClose;

        public UpdateCompleteViewModel()
        {
            UpdateCompleteModel = new UpdateCompleteModel();
        }

        public static UpdateCompleteModel UpdateCompleteModel { get; set; }

        public ICommand AcceptCommand
        {
            get
            {
                return _accept_command ?? (_accept_command = new CommandHandler(() => UpdateCompleteCommands.RestartToscaServerDependencies(), () => true));
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

    }
}
