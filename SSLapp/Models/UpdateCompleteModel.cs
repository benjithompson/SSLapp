using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SSLapp.Models
{
    class UpdateCompleteModel : INotifyPropertyChanged
    {
        private string _button;

        public UpdateCompleteModel()
        {
            _button = "Ok";
        }
        public string Button
        {
            get { return _button; }
            set
            {
                _button = value;
                NotifyPropertyChanged(nameof(Button));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
