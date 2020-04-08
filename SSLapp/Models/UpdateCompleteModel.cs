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
        private string _closeButton;
        private string _textBlockMessage;
        private bool _acceptButtonVisible;
        private bool _declineButtonVisible;
        private bool _closeButtonVisible;

        public UpdateCompleteModel()
        {
            AcceptButton = "Yes";
            DeclineButton = "No";
            CloseButton = "Close";
            TextBlockMessage = "Do you want to restart Tricentis Services?";
            AcceptButtonVisible = true;
            DeclineButtonVisible = true;
            CloseButtonVisible = false;

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
        public string CloseButton
        {
            get { return _closeButton; }
            set
            {
                _closeButton = value;
                NotifyPropertyChanged(nameof(CloseButton));
            }
        }
        public string TextBlockMessage
        {
            get { return _textBlockMessage; }
            set
            {
                _textBlockMessage = value;
                NotifyPropertyChanged(nameof(TextBlockMessage));
            }
        }
        public bool AcceptButtonVisible
        {
            get { return _acceptButtonVisible; }
            set
            {
                _acceptButtonVisible = value;
                NotifyPropertyChanged(nameof(AcceptButtonVisible));
            }
        }

        public bool DeclineButtonVisible
        {
            get { return _declineButtonVisible; }
            set
            {
                _declineButtonVisible = value;
                NotifyPropertyChanged(nameof(DeclineButtonVisible));
            }
        }
        public bool CloseButtonVisible
        {
            get { return _closeButtonVisible; }
            set
            {
                _closeButtonVisible = value;
                NotifyPropertyChanged(nameof(CloseButtonVisible));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
