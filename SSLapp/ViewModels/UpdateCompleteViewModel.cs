using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SSLapp.Commands;
using SSLapp.Models;
using SSLapp.Views;

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
                return _accept_command ?? (_accept_command = new CommandHandler(() => Commands.Commands.AcceptCommand(), () => true));
            }
        }

        public ICommand DeclineCommand
        {
            get
            {
                return _decline_command ?? (_decline_command = new CommandHandler(() => DeclineCommandHandler(), () => true));
            }
        }

        private void DeclineCommandHandler()
        {
            OnRequestClose(this, new EventArgs());
        }

    }
}
