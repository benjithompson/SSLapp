using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SSLapp.Models
{
    class UpdateCompleteModel : INotifyPropertyChanged
    {
        private string _acceptButton;
        private string _declineButton;

        public UpdateCompleteModel()
        {
            _acceptButton = "Ok";
            _declineButton = "Close";
        }
        public string AcceptButton
        {
            get { return _acceptButton; }
            set
            {
                _acceptButton = value;
                NotifyPropertyChanged(nameof(AcceptButton));
            }
        }
        public string DeclineButton
        {
            get { return _declineButton; }
            set
            {
                _declineButton = value;
                NotifyPropertyChanged(nameof(DeclineButton));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
