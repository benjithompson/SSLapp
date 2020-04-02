using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SSLapp.ViewModels;

namespace SSLapp.Views
{
    /// <summary>
    /// Interaction logic for UpdateCompleteView.xaml
    /// </summary>
    public partial class UpdateCompleteView : Window
    {
        public UpdateCompleteView()
        {
            InitializeComponent();
            DataContext = new UpdateCompleteViewModel();
        }
    }
}
